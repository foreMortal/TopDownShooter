using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private RecordScriptableObject recordObject;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyUnit regular, fast, guarded;

    private List<EnemyUnit> regularEnemies = new List<EnemyUnit>();
    private List<EnemyUnit> fastEnemies = new List<EnemyUnit>();
    private List<EnemyUnit> guardedEnemies = new List<EnemyUnit>();

    private List<EnemyUnit> activeEnemies = new List<EnemyUnit>();

    private Camera cam;
    private float timer, nextReduce, spawnTime = 2f, minimumSpawnTime = 0.5f;
    private float camWidth, camHeight, fieldRangeX, fieldRangeY;
    private EnemyUnit newEnemy;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        camWidth = Screen.width;
        camHeight = Screen.height;
        fieldRangeX = 20f;
        fieldRangeY = 15f;

        Spawn();
        nextReduce = Time.time + 10f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(Time.time >= nextReduce)
        {
            if(spawnTime > minimumSpawnTime)
                spawnTime -= 0.1f;
            else
                spawnTime = minimumSpawnTime;
            nextReduce = Time.time + 10f;
        }
        if (timer >= spawnTime)
        {
            Spawn();
            timer = 0f;
        }
    }

    private void Spawn()
    {
        int num = Random.Range(1, 101);

        if(num <= 60)//обычный
        {
            if(regularEnemies.Count > 0)
            {
                newEnemy = regularEnemies[0];
                regularEnemies.RemoveAt(0);
            }
            else
            {
                newEnemy = Instantiate(regular);
                newEnemy.SelfAwake(this);
            }
        }
        else if (num <= 90)//шустрый
        {
            if (fastEnemies.Count > 0)
            {
                newEnemy = fastEnemies[0];
                fastEnemies.RemoveAt(0);
            }
            else
            {
                newEnemy = Instantiate(fast);
                newEnemy.SelfAwake(this);
            }
        }
        else//бронированный 
        {
            if (guardedEnemies.Count > 0)
            {
                newEnemy = guardedEnemies[0];
                guardedEnemies.RemoveAt(0);
            }
            else
            {
                newEnemy = Instantiate(guarded);
                newEnemy.SelfAwake(this);
            }
        }//взять из пула или создать нового врага а затем обновить его
        newEnemy.ResetEnemy(player, GetNewPosition());
        activeEnemies.Add(newEnemy);
    }

    public void EnemyDied(EnemyUnit enemy)
    {
        enemy.gameObject.SetActive(false);
        recordObject.ChangeCurrentPoints(enemy.Points);
        activeEnemies.Remove(enemy);

        switch (enemy.EnemyType)//в зависимости от варага обновить очки
        {
            case EnemyType.Regular:
                regularEnemies.Add(enemy);
                break;
            case EnemyType.Fast:
                fastEnemies.Add(enemy);
                break;
            case EnemyType.Guarded:
                guardedEnemies.Add(enemy);
                break;
        }
    }

    private Vector3 GetNewPosition()
    {
        int side = Random.Range(0, 4);
        Vector3 newVec = new Vector3();
        switch(side)
        {
            case 0:
                newVec = SpawnBottom();
                break;
            case 1:
                newVec = SpawnTop();
                break;
            case 2:
                newVec = SpawnRight();
                break;
            case 3:
                newVec = SpawnLeft();
                break;
        }
        if (newVec != Vector3.zero)
            return newVec;
        else
            return GetNewPosition();
    }

    private Vector3 SpawnLeft()
    {
        Vector3 bottomPoint = cam.ScreenToWorldPoint(Vector3.zero);// нижняя позиция экрана 
        Vector3 topPoint = cam.ScreenToWorldPoint(new(0f, camHeight, 0f));// верхняя позиция экрана
        if (bottomPoint.x > -fieldRangeX + 3f)// если игрок не слишкм близок к левой стене
        {// выбрать позицию между левым краем карты и левом краем экрана по Х и позицию между нижней и верхней частью экрана по Y
            return  new Vector3(Random.Range(-fieldRangeX + 1f, bottomPoint.x - 1f), Random.Range(bottomPoint.y, topPoint.y));
        }
        else// если слишком близко, выбрать другую позицию
        {
            return Vector3.zero;
        }
    }

    private Vector3 SpawnRight()
    {
        Vector3 bottomPoint = cam.ScreenToWorldPoint(new(camWidth, 0f, 0f));
        Vector3 topPoint = cam.ScreenToWorldPoint(new(0f, camHeight, 0f));
        if (bottomPoint.x < fieldRangeX - 3f)
        {
            return new Vector3(Random.Range(bottomPoint.x + 1f, fieldRangeX - 1f), Random.Range(bottomPoint.y, topPoint.y));
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 SpawnTop()
    {
        Vector3 leftPoint = cam.ScreenToWorldPoint(new(0, camHeight, 0f));
        Vector3 rightPoint = cam.ScreenToWorldPoint(new(camWidth, camHeight, 0f));
        if (leftPoint.y < fieldRangeY - 3f)
        {
            return new Vector3(Random.Range(leftPoint.x, rightPoint.x), Random.Range(leftPoint.y + 1f, fieldRangeY - 1f));
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 SpawnBottom()
    {
        Vector3 leftPoint = cam.ScreenToWorldPoint(new(0, 0f, 0f));
        Vector3 rightPoint = cam.ScreenToWorldPoint(new(camWidth, 0f, 0f));
        if (leftPoint.y > -fieldRangeY + 3f)
        {
            return new Vector3(Random.Range(leftPoint.x, rightPoint.x), Random.Range(leftPoint.y - 1f,  -fieldRangeY + 1f));
        }
        else
        {
            return Vector3.zero;
        }
    }
}

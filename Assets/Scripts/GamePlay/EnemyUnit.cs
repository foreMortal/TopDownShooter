using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private float points, Health, Speed;
    [SerializeField] private EnemyType enemyType;
    private EnemyManager manager;
    private EnemyFollows moveScript;
    private Health health;

    public float Points { get { return points; } }
    public EnemyType EnemyType { get { return enemyType;} }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().GetHited(1f);
        }
    }

    public void SelfAwake(EnemyManager manager)
    {
        this.manager = manager;
        moveScript = GetComponent<EnemyFollows>();
        health = GetComponent<Health>();

        health.maxHealth = Health;
        moveScript.speed = Speed;

        health.DiedEvent += EnemyDie;
    }

    public void ResetEnemy(Transform player, Vector3 SpawnPos)
    {
        gameObject.SetActive(true);
        transform.position = SpawnPos;
        moveScript.player = player;
        health.Restore();
        moveScript.Active = true;
    }

    private void EnemyDie()
    {
        manager.EnemyDied(this);
        moveScript.Active = false;
    }
}

public enum EnemyType
{
    Regular,
    Fast, 
    Guarded,
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Vector3 bulletSize, position, rotation;
    [SerializeField] protected float damage, shotsPerSecond, bulletsStartSpeed, bulletsMaxDistance, lifeTime;

    public Vector3 Position { get { return position; } }
    public Vector3 Rotation { get { return rotation; } }

    protected List<BulletInfo> activeBullets = new List<BulletInfo>();
    protected List<BulletInfo> removeList = new List<BulletInfo>();
    protected List<BulletInfo> hidedBullets = new List<BulletInfo>();

    private RaycastHit2D hit;
    protected LayerMask mask;

    private void Awake()
    {
        GetComponentInParent<PlayerShooting>().TimeBetweenShots = 1f / shotsPerSecond;
        mask += 1 << 7;
    }

    public virtual void ChangeWeapon()
    {
        if (activeBullets.Count > 0)
        {
            foreach (var b in activeBullets)
            {
                Destroy(b.bulletInstance.gameObject);
            }
            activeBullets.Clear();
        }
        if (hidedBullets.Count > 0)
        {
            foreach (var b in hidedBullets)
            {
                Destroy(b.bulletInstance.gameObject);
            }
            hidedBullets.Clear();
        }
        if (removeList.Count > 0)
        {
            foreach (var b in removeList)
            {
                Destroy(b.bulletInstance.gameObject);
            }
            removeList.Clear();
        }
    }

    protected virtual void Update()
    {// просчёт позиции пули и её коллизий 
        // эффективней и менее ресурсозатратно чем проверка колизий через хитбоксы 
        if(activeBullets.Count > 0)
        {
            foreach (var bullet in activeBullets)
            {
                if(bullet.timer >= bullet.lifeTime)
                {
                    bullet.timedOut = true;
                    removeList.Add(bullet);
                }
                float speed = bullet.Speed;
                // просчёт нового положения пули
                Vector3 nextPosition = bullet.bulletInstance.position + bullet.fireDirection * (speed * Time.deltaTime);
                // просчёт пройденного растояния на коллизии 
                hit = Physics2D.BoxCast(bullet.bulletInstance.position, bulletSize, 0f, bullet.fireDirection, speed * Time.deltaTime, mask);
                if (hit)
                {
                    hit.collider.GetComponent<Health>().GetHited(damage);
                    if(!bullet.timedOut)
                        removeList.Add(bullet);
                }
                bullet.bulletInstance.transform.position = nextPosition;

                bullet.timer += Time.deltaTime;// если пуля ни с чем не столкнулась, то пропадёт со временем
            }
        }
        if(removeList.Count > 0)
        {
            foreach(var bul in removeList)
            {
                bul.bulletInstance.gameObject.SetActive(false);
                activeBullets.Remove(bul);
                hidedBullets.Add(bul);
            }
            removeList.Clear();
        }
    }

    public virtual void Fire(Vector3 fireDirection, Vector3 range)
    {
        if (hidedBullets.Count > 0)// взять пулю из пула
        {
            hidedBullets[0].Reset(fireDirection, firePoint.position, Quaternion.LookRotation(fireDirection, -Vector3.forward));
            activeBullets.Add(hidedBullets[0]);
            hidedBullets.RemoveAt(0);
        }
        else// создать новую если пул пуст
        {
            BulletInfo bullet = new()
            {
                bulletInstance = Instantiate(BulletPrefab, firePoint.position, Quaternion.LookRotation(fireDirection, -Vector3.forward)).transform,
                maxDistance = bulletsMaxDistance,
                Speed = bulletsStartSpeed,
                fireDirection = fireDirection,
                distanceFlied = 0,
            };
            activeBullets.Add(bullet);
        }
    }
}

public class GranadeInfo : BulletInfo// расширение для гранатомёта
{
    public Animator animator;
}

public class BulletInfo// класс для хранения информации о пуле
{
    public Transform bulletInstance;
    protected float startSpeed;
    public float damage, maxDistance = -1f, distanceFlied = 0f, lifeTime = 0.5f, timer;
    public Vector3 fireDirection;
    public bool timedOut;

    public void Reset(Vector3 dir, Vector3 pos, Quaternion rot)
    {
        bulletInstance.position = pos;
        bulletInstance.rotation = rot;
        bulletInstance.gameObject.SetActive(true);
        fireDirection = dir;
        distanceFlied = 0f;
        timer = 0f;
        timedOut = false;
    }

    public float Speed// если у оружие ограниченная дальность стрельбы, то пуля будет терять скорость начиная с половины пути
    {// если дальность неограниченна то скорость постоянная 
        get
        {
            if (maxDistance <= 0)
            {
                return startSpeed;
            }
            else
            {
                if (distanceFlied < maxDistance / 2)
                {
                    distanceFlied += startSpeed * Time.deltaTime;
                    return startSpeed;
                }
                else
                {
                    float fliedD = distanceFlied - maxDistance / 2;
                    float halfDist = maxDistance / 2;

                    float t = fliedD / halfDist;
                    t = 1 - t;
                    distanceFlied += startSpeed * t * Time.deltaTime;
                    return startSpeed * t;
                }
            }
        }
        set
        {
            startSpeed = value;
        }
    }
}

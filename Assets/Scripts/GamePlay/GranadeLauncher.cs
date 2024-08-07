using System.Collections.Generic;
using UnityEngine;

public class GranadeLauncher : WeaponParent
{
    public ContactFilter2D contactFilter;
    protected List<BulletInfo> buffer = new List<BulletInfo>();
    private List<Collider2D> hits = new List<Collider2D>();

    private void Start()
    {
        contactFilter.SetLayerMask(mask);
    }

    public override void ChangeWeapon()
    {
        base.ChangeWeapon();
        if(buffer.Count > 0)
        {
            foreach(var b in buffer)
            {
                Destroy(b.bulletInstance.gameObject);
            }
            buffer.Clear();
        }
    }

    public override void Fire(Vector3 fireDirection, Vector3 fireRange)
    {
        if (hidedBullets.Count > 0)
        {
            hidedBullets[0].Reset(fireDirection, firePoint.position, Quaternion.LookRotation(fireDirection, -Vector3.forward));
            hidedBullets[0].maxDistance = fireRange.magnitude;
            ((GranadeInfo)hidedBullets[0]).animator.Play("Start");
            activeBullets.Add(hidedBullets[0]);
            hidedBullets.RemoveAt(0);
        }
        else
        {
            GranadeInfo bullet = new()
            {
                bulletInstance = Instantiate(BulletPrefab, firePoint.position, Quaternion.LookRotation(fireDirection, -Vector3.forward)).transform,
                lifeTime = 1f,
                maxDistance = fireRange.magnitude,
                Speed = bulletsStartSpeed,
                fireDirection = fireDirection,
                distanceFlied = 0,
            };
            bullet.animator = bullet.bulletInstance.gameObject.GetComponent<Animator>();
            activeBullets.Add(bullet);
        }
    }

    protected override void Update()
    {
        if (activeBullets.Count > 0)
        {
            foreach (var bullet in activeBullets)
            {
                float speed = bullet.Speed;
                Vector3 nextPosition = bullet.bulletInstance.position + bullet.fireDirection * (speed * Time.deltaTime);
                if (bullet.distanceFlied >= bullet.maxDistance - 0.1f)
                {
                    ((GranadeInfo)bullet).animator.Play("Explosion");
                    int t = Physics2D.OverlapCircle(nextPosition, 1.5f, contactFilter, hits);
                    if (t > 0)// просчитать взрыв
                    {
                        foreach(var hit in hits)
                        {
                            hit.transform.GetComponent<Health>().GetHited(damage);
                        }
                    }
                    buffer.Add(bullet);
                }
                bullet.bulletInstance.transform.position = nextPosition;
            }
        }
        if (buffer.Count > 0)// буффер для пуль, чтобы успела проиграться анимация 
        {
            foreach (var bul in buffer)
            {
                if (!bul.timedOut)
                {
                    activeBullets.Remove(bul);
                    bul.timedOut = true;
                }
                else if (bul.lifeTime > bul.timer)
                {
                    bul.timer += Time.deltaTime;
                }
                else
                {
                    removeList.Add(bul);
                }
            }
        }
        if(removeList.Count > 0)
        {
            foreach (var bul in removeList)
            {
                buffer.Remove(bul);
                bul.bulletInstance.gameObject.SetActive(false);
                hidedBullets.Add(bul);
            }
            removeList.Clear();
        }
    }
}

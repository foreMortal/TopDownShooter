using System.IO.Pipes;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;

    private string weaponName = "Pistol";
    private WeaponParent weapon;

    Ray clickPos;
    RaycastHit hit;

    private float speed = 180f, timeBetweenShots, timer;
    private bool rotate, fire;

    public string WeaponName { get { return weaponName; } set { weaponName = value; } }

    public float TimeBetweenShots
    {
        set
        {
            timeBetweenShots = value;
            timer = timeBetweenShots;
        }
    }

    private void Awake()
    {
        weapon = GetComponentInChildren<WeaponParent>();
    }

    private void Update()
    {
        if (timer < timeBetweenShots)
            timer += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            clickPos = cam.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(clickPos, out hit, 15f, 1 << 6);

            rotate = true;// после выстрела персонаж продолжает поворачивается в точку прошлого выстрела
            if (timer >= timeBetweenShots)
                fire = true;
        }
        if (rotate)
        {
            Vector3 fireDirection = hit.point - transform.position;
            fireDirection.z = 0;
            Vector3 fireRange = fireDirection;
            fireDirection.Normalize();

            Quaternion rotateTo = Quaternion.LookRotation(fireDirection, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, speed * Time.deltaTime);
            
            if(Vector3.Dot(transform.forward, fireDirection) >= 0.95)
            {
                if (fire)
                {
                    timer -= timeBetweenShots;
                    fire = false;
                    weapon.Fire(fireDirection, fireRange);
                }
            }
        }
    }

    public void ChangeWeapon(WeaponParent newWeapon)
    {
        weapon.ChangeWeapon();
        Destroy(weapon.gameObject);
        newWeapon.transform.SetLocalPositionAndRotation(newWeapon.Position, Quaternion.Euler(newWeapon.Rotation));
        weapon = newWeapon;
    }
}

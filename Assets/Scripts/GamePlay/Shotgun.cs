using UnityEngine;

public class Shotgun : WeaponParent
{
    public override void Fire(Vector3 fireDirection, Vector3 _)
    {
        for(int i = 0; i < 5; i++)
        {
            Vector3 t = fireDirection + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            base.Fire(t.normalized, Vector3.zero);
        }
    }
}

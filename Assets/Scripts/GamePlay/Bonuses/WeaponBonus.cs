using UnityEngine;
public class WeaponBonus : BonusParent
{
    [SerializeField] private WeaponParent weapon;
    [SerializeField] private string WeaponName;

    public override void AddBonus(PlayerManager manager)
    {
        base.AddBonus(manager);
        manager.PlayerShooting.WeaponName = WeaponName;
        manager.PlayerShooting.ChangeWeapon(Instantiate(weapon, manager.transform));
    }
}

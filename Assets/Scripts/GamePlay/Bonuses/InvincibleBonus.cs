public class InvincibleBonus : PlayerBonus
{
    public override void AddBonus(PlayerManager manager)
    {
        base.AddBonus(manager);
        manager.Health.CanBeDamaged = false;
    }

    public override void RemoveBonus(PlayerManager manager)
    {
        manager.Health.CanBeDamaged = true;
    }
}

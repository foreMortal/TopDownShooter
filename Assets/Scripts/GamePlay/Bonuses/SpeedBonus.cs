public class SpeedBonus : PlayerBonus
{
    public override void AddBonus(PlayerManager manager)
    {
        base.AddBonus(manager);
        manager.PlayerMovement.ChangeModifier(0, 1.5f);
    }

    public override void RemoveBonus(PlayerManager manager)
    {
        playerManager.PlayerMovement.ChangeModifier(1, 1.5f);
    }
}

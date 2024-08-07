public class SlowZone : VoidZoneParent
{
    protected override void AffectPlayer(PlayerManager manager)
    {
        manager.PlayerMovement.ChangeModifier(0, 0.6f);
    }
    protected override void RemoveAffect(PlayerManager manager)
    {
        manager.PlayerMovement.ChangeModifier(1, 0.6f);
    }
}

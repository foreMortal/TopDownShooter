public class DeathZone : VoidZoneParent
{
    protected override void AffectPlayer(PlayerManager manager)
    {
        manager.Health.UnitDie();
    }
}

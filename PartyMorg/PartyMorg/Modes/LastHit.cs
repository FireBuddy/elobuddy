using EloBuddy.SDK;

namespace PartyMorg.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted() => Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);

        public override void Execute()
        {
        }
    }
}
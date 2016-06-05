using EloBuddy.SDK;

namespace PartyJanna.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted() => Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);

        public override void Execute()
        {
        }
    }
}
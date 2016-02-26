using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using Settings = PartyJanna.Config.Modes.Combo;

namespace PartyJanna.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            Q.Range = (uint)Settings.QUseRange;

            var target = GetTarget(W, DamageType.Magical);

            if (target != null && Settings.UseW)
            {
                W.Cast(target);
            }

            target = GetTarget(Q, DamageType.Magical);

            if (target != null && Settings.UseQ)
            {
                Q.Cast(Q.GetPrediction(target).CastPosition);
            }
        }
    }
}

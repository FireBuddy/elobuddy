using System;
using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyJanna.Config.Settings.Combo;

namespace PartyJanna.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted() => Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);

        public override void Execute()
        {
            var target = GetTarget(W, DamageType.Magical);

            if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseW)
                W.Cast(target);

            target = GetTarget(Q, DamageType.Magical);

            if (target == null || !target.IsTargetable || target.HasBuffOfType(BuffType.SpellImmunity) || !Settings.UseQ ||
                target.IsDead) return;

            var pred = Q.GetPrediction(target);

            if (Config.Settings.Humanizer.QCastDelayEnabled)
                Core.DelayAction(() => { Q.Cast(pred.CastPosition); },
                    Config.Settings.Humanizer.QRndmDelay
                        ? new Random().Next(250, Config.Settings.Humanizer.QCastDelay)
                        : Config.Settings.Humanizer.QCastDelay);
            else
                Q.Cast(pred.CastPosition);
        }
    }
}
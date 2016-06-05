using System;
using EloBuddy;
using EloBuddy.SDK;
using static PartyMorg.Config.Settings;
using Settings = PartyMorg.Config.Settings.Flee;

namespace PartyMorg.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted() => Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);

        public override void Execute()
        {
            var target = GetTarget(Q, DamageType.Magical);

            PredictionResult pred;

            if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseQ &&
                !target.IsDead)
            {
                pred = Q.GetPrediction(target);

                if (Humanizer.QCastDelayEnabled)
                {
                    if (pred.HitChancePercent >= Settings.QMinHitChance)
                        Core.DelayAction(() => { Q.Cast(pred.CastPosition); },
                            Humanizer.QRndmDelay
                                ? new Random().Next(250, Humanizer.QCastDelay)
                                : Humanizer.QCastDelay);
                }
                else
                {
                    if (pred.HitChancePercent >= Settings.QMinHitChance)
                    {
                        Q.Cast(pred.CastPosition);
                    }
                }
            }

            if (Settings.WImmobileOnly)
            {
                if (target == null || !target.IsTargetable || target.HasBuffOfType(BuffType.SpellImmunity) ||
                    !Settings.UseW || target.IsDead || !Player.Instance.IsInRange(target, W.Range) ||
                    !IsImmobile(target))
                    return;

                pred = W.GetPrediction(target);

                W.Cast(pred.CastPosition);
            }
            else
            {
                if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) &&
                    Settings.UseW && !target.IsDead && Player.Instance.IsInRange(target, W.Range))
                {
                    pred = W.GetPrediction(target);

                    if (Settings.UseQBeforeW)
                    {
                        if (Q.IsOnCooldown)
                        {
                            W.Cast(pred.CastPosition);
                        }
                    }
                    else
                    {
                        W.Cast(pred.CastPosition);
                    }
                }
            }
        }
    }
}
using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Diagnostics;
using Humanizer = PartyMorg.Config.Settings.Humanizer;
using Settings = PartyMorg.Config.Settings.Flee;

namespace PartyMorg.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public static Stopwatch stopwatch = new Stopwatch();

        public override void Execute()
        {
            var target = GetTarget(Q, DamageType.Magical);
            PredictionResult pred;

            if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseQ && !target.IsDead)
            {
                pred = Q.GetPrediction(target);

                if (Humanizer.QCastDelayEnabled)
                {
                    if (Humanizer.QRndmDelay)
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.QCastDelay))
                        {
                            if (pred.HitChancePercent >= Settings.QMinHitChance)
                            {
                                Q.Cast(pred.CastPosition);
                            }

                            stopwatch.Reset();
                        }
                    }
                    else
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                        {
                            if (pred.HitChancePercent >= Settings.QMinHitChance)
                            {
                                Q.Cast(pred.CastPosition);
                            }

                            stopwatch.Reset();
                        }
                    }
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
                if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseW && !target.IsDead && Player.Instance.IsInRange(target, W.Range) && Immobile(target))
                {
                    pred = W.GetPrediction(target);

                    W.Cast(pred.CastPosition);
                }
            }
            else
            {
                if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseW && !target.IsDead && Player.Instance.IsInRange(target, W.Range))
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

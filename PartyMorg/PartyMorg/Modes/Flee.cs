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

            if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseQ)
            {
                if (Humanizer.QCastDelayEnabled)
                {
                    if (Humanizer.QRndmDelay)
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.QCastDelay))
                        {
                            var pred = Q.GetPrediction(target);

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
                            var pred = Q.GetPrediction(target);

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
                    var pred = Q.GetPrediction(target);

                    if (pred.HitChancePercent >= Settings.QMinHitChance)
                    {
                        Q.Cast(pred.CastPosition);
                    }
                }
            }

            if (target != null && Immobile(target) && Player.Instance.IsInRange(target, W.Range))
            {
                W.Cast(W.GetPrediction(target).CastPosition);
            }
        }
    }
}

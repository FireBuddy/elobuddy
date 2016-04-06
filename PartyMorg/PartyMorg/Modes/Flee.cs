using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyMorg.Config.Settings.Flee;
using Humanizer = PartyMorg.Config.Settings.Humanizer;
using System.Diagnostics;
using System;

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
            //Q.Range = (uint)Settings.QUseRange;

            var target = GetTarget(W, DamageType.Magical);

            if (target != null && Settings.UseW)
            {
                W.Cast(target);
            }

            target = GetTarget(Q, DamageType.Magical);

            if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseQ)
            {
                if (Humanizer.QCastDelayEnabled)
                {
                    if (Humanizer.QRndmDelay)
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.QCastDelay))
                        {
                            Q.Cast(Q.GetPrediction(target).CastPosition);
                            stopwatch.Reset();
                        }
                    }
                    else
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                        {
                            Q.Cast(Q.GetPrediction(target).CastPosition);
                            stopwatch.Reset();
                        }
                    }
                }
                else
                {
                    Q.Cast(Q.GetPrediction(target).CastPosition);
                }
            }
        }
    }
}

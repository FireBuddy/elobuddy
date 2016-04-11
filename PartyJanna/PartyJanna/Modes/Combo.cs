using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Diagnostics;
using Humanizer = PartyJanna.Config.Settings.Humanizer;
using Settings = PartyJanna.Config.Settings.Combo;

namespace PartyJanna.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
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

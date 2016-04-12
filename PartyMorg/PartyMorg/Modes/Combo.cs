using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Diagnostics;
using Humanizer = PartyMorg.Config.Settings.Humanizer;
using Settings = PartyMorg.Config.Settings.Combo;

namespace PartyMorg.Modes
{
    public sealed class Combo : ModeBase
    {
        public static Item zhonyasHourglass { get; private set; }
        public static Spell.Targeted flashSpell { get; private set; }
        static int enemiesFaced = 0;

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public static Stopwatch stopwatch = new Stopwatch();

        public override void Execute()
        {
            zhonyasHourglass = new Item(3157);
            flashSpell = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerflash"), uint.MaxValue);

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
                if (target != null && Immobile(target) && Player.Instance.IsInRange(target, W.Range) && !target.IsDead)
                {
                    W.Cast(W.GetPrediction(target).CastPosition);
                }
            }
            else
            {
                target = GetTarget(W, DamageType.Magical);
                pred = W.GetPrediction(target);

                if (target != null && Player.Instance.IsInRange(target, W.Range) && !target.IsDead)
                {
                    W.Cast(W.GetPrediction(target).CastPosition);
                }
            }

            target = GetTarget(R, DamageType.Magical);

            if (R.IsReady() && Settings.UseR)
            {
                if (Player.Instance.CountEnemiesInRange(Settings.UltMinRange) == 0 && Settings.FlashUlt)
                {
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        if (Player.Instance.IsFacing(enemy))
                        {
                            enemiesFaced++;
                        }

                        if (enemiesFaced >= Settings.RMinEnemies && Player.Instance.CountEnemiesInRange(Settings.UltMinRange + 400) >= Settings.RMinEnemies)
                        {
                            flashSpell.Cast(enemy.Position);

                            R.Cast();

                            if (Settings.UltZhonya)
                            {
                                zhonyasHourglass.Cast();
                            }

                            enemiesFaced = 0;
                        }
                    }
                }
                else
                {
                    if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseR && Player.Instance.CountEnemiesInRange(Settings.UltMinRange) >= Settings.RMinEnemies && !target.IsDead)
                    {
                        if (Humanizer.RCastDelayEnabled)
                        {
                            if (Humanizer.RRndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.RCastDelay))
                                {
                                    R.Cast();

                                    if (Settings.UltZhonya)
                                    {
                                        zhonyasHourglass.Cast();
                                    }

                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.RCastDelay)
                                {
                                    R.Cast();

                                    if (Settings.UltZhonya)
                                    {
                                        zhonyasHourglass.Cast();
                                    }

                                    stopwatch.Reset();
                                }
                            }
                        }
                        else
                        {
                            R.Cast();

                            if (Settings.UltZhonya)
                            {
                                zhonyasHourglass.Cast();
                            }
                        }
                    }
                }
            }
        }
    }
}

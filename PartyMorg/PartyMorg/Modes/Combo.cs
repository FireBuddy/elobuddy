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
        private static Item zhonyasHourglass = new Item(3157);

        public static Spell.Skillshot flashSpell { get; private set; }

        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public static Stopwatch stopwatch = new Stopwatch();

        public override void Execute()
        {
            flashSpell = new Spell.Skillshot(Player.Instance.GetSpellSlotFromName("summonerflash"), 425, EloBuddy.SDK.Enumerations.SkillShotType.Linear);

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

            target = GetTarget(R, DamageType.Magical);

            if (R.IsReady() && Settings.UseR && target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && !target.IsDead)
            {
                if (Player.Instance.CountEnemiesInRange(Settings.UltMinRange) == 0 && Settings.FlashUlt)
                {
                    int enemiesFaced = 0;

                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        if (Player.Instance.IsFacing(enemy))
                        {
                            enemiesFaced++;
                        }

                        if (enemiesFaced >= Settings.RMinEnemies && Player.Instance.CountEnemiesInRange(Settings.UltMinRange + flashSpell.Range) >= Settings.RMinEnemies)
                        {
                            flashSpell.Cast(Player.Instance.Position.Extend(enemy.Position, flashSpell.Range).To3D());

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

using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyMorg.Config.Settings.Combo;
using Humanizer = PartyMorg.Config.Settings.Humanizer;
using System.Diagnostics;
using System;
using System.Linq;

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

        public static bool Immobile(Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Stun) ||
                   target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Snare) ||
                   target.HasBuffOfType(BuffType.Taunt) || target.HasBuffOfType(BuffType.Suppression);
        }

        public override void Execute()
        {
            //Q.Range = (uint)Settings.QUseRange;

            zhonyasHourglass = new Item(3157);
            flashSpell = new Spell.Targeted(Player.Instance.GetSpellSlotFromName("summonerflash"), uint.MaxValue);

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

            if (Immobile(target) && Player.Instance.IsInRange(target, W.Range))
            {
                W.Cast(target.Position);
            }

            target = GetTarget(R, DamageType.Magical);

            if (R.IsReady())
            {
                if (Player.Instance.CountEnemiesInRange(Settings.UltMinRange) == 0 && Settings.FlashUlt)
                {
                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        //for (int i = 0; i < EntityManager.Heroes.Enemies.Count; i++)
                        //{
                            if (Player.Instance.IsFacing(enemy))
                            {
                                enemiesFaced++;
                            }
                        //}

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
                    if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseR && Player.Instance.CountEnemiesInRange(Settings.UltMinRange) >= Settings.RMinEnemies)
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

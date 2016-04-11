using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using _Interrupter = PartyMorg.Config.Settings.Interrupter;
using AntiGapcloser = PartyMorg.Config.Settings.AntiGapcloser;
using AutoShield = PartyMorg.Config.Settings.AutoShield;
using Humanizer = PartyMorg.Config.Settings.Humanizer;

namespace PartyMorg
{
    public static class Events
    {
        public static List<AIHeroClient> priorAllyOrder { get; private set; }
        public static List<AIHeroClient> hpAllyOrder { get; private set; }
        public static int highestPriority { get; private set; }
        public static float lowestHP { get; private set; }
        public static Stopwatch stopwatch = new Stopwatch();

        static Events()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        public static void Initialize() { }

        private static void CastShield(Obj_AI_Base target)
        {
            if (Humanizer.ECastDelayEnabled)
            {
                if (Humanizer.ERndmDelay)
                {
                    stopwatch.Start();

                    if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                    {
                        SpellManager.E.Cast(target);
                        stopwatch.Reset();
                    }
                }
                else
                {
                    stopwatch.Start();

                    if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                    {
                        SpellManager.E.Cast(target);
                        stopwatch.Reset();
                    }
                }
            }
            else
            {
                SpellManager.E.Cast(target);
            }
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy || !AntiGapcloser.AntiGap || Player.Instance.IsRecalling())
            { return; }

            foreach (var ally in EntityManager.Heroes.Allies)
            {
                if (sender.IsFacing(ally) && SpellManager.Q.IsInRange(sender.Position))
                {
                    if (Humanizer.QCastDelayEnabled)
                    {
                        if (Humanizer.QRndmDelay)
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.QCastDelay))
                            {
                                SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                                stopwatch.Reset();
                            }
                        }
                        else
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                            {
                                SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                                stopwatch.Reset();
                            }
                        }
                    }
                    else
                    {
                        SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                    }
                }
            }
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy || Player.Instance.IsRecalling())
            { return; }

            if (e.DangerLevel == DangerLevel.High)
            {
                if (_Interrupter.QInterruptDangerous && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
                {
                    if (Humanizer.QCastDelayEnabled)
                    {
                        if (Humanizer.QRndmDelay)
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.QCastDelay))
                            {
                                SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                                stopwatch.Reset();
                            }
                        }
                        else
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                            {
                                SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                                stopwatch.Reset();
                            }
                        }
                    }
                    else
                    {
                        SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                    }
                }
            }
            else
            {
                if (_Interrupter.QInterrupt && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
                {
                    if (Humanizer.QCastDelayEnabled)
                    {
                        if (Humanizer.QRndmDelay)
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.QCastDelay))
                            {
                                SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                                stopwatch.Reset();
                            }
                        }
                        else
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                            {
                                SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                                stopwatch.Reset();
                            }
                        }
                    }
                    else
                    {
                        SpellManager.Q.Cast(SpellManager.Q.GetPrediction(sender).CastPosition);
                    }
                }
            }
        }

        public static System.Drawing.RectangleF GetRectangle(PointF p1, PointF p2)
        {
            float top = Math.Min(p1.Y, p2.Y);
            float bottom = Math.Max(p1.Y, p2.Y);
            float left = Math.Min(p1.X, p2.X);
            float right = Math.Max(p1.X, p2.X);

            System.Drawing.RectangleF rect = System.Drawing.RectangleF.FromLTRB(left, top, right, bottom);

            return rect;
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || Player.Instance.IsRecalling())
            { return; }

            priorAllyOrder = new List<AIHeroClient>();

            hpAllyOrder = new List<AIHeroClient>();

            highestPriority = 0;

            lowestHP = int.MaxValue;

            if (AutoShield.PriorMode == 1)
            {
                foreach (var slider in AutoShield.Sliders)
                {
                    if (slider.CurrentValue >= highestPriority)
                    {
                        highestPriority = slider.CurrentValue;

                        foreach (var ally in AutoShield.Heros)
                        {
                            if (slider.VisibleName.Contains(ally.ChampionName))
                            {
                                priorAllyOrder.Insert(0, ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (var ally in AutoShield.Heros)
                        {
                            if (slider.VisibleName.Contains(ally.ChampionName))
                            {
                                priorAllyOrder.Add(ally);
                            }
                        }
                    }
                }

                foreach (var ally in priorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                    {
                        foreach (var shieldThisSpell in AutoShield.ShieldSpellList.Where(s => s.DisplayName.Contains(args.SData.Name) && s.CurrentValue))
                        {
                            if (args.Target == ally)
                            {
                                CastShield(ally);
                            }
                            else
                            {
                                if (Prediction.Position.PredictUnitPosition(ally, 250).IsInRange(args.End, MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 1]))
                                {
                                    CastShield(ally);
                                }

                                if (sender.IsFacing(ally) && Prediction.Position.PredictUnitPosition(ally, 250).IsInRange(sender, MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 0]))
                                {
                                    CastShield(ally);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (ally.Health <= lowestHP)
                    {
                        lowestHP = ally.Health;
                        hpAllyOrder.Insert(0, ally);
                    }
                    else
                    {
                        hpAllyOrder.Add(ally);
                    }
                }

                foreach (var ally in hpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(a => a.DisplayName.Contains(ally.ChampionName) && a.CurrentValue))
                    {
                        foreach (var shieldThisSpell in AutoShield.ShieldSpellList.Where(s => s.DisplayName.Contains(args.SData.Name) && s.CurrentValue))
                        {
                            if (args.Target == ally)
                            {
                                CastShield(ally);
                            }
                            else
                            {
                                if (Prediction.Position.PredictUnitPosition(ally, 250).IsInRange(args.End, MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 1]))
                                {
                                    CastShield(ally);
                                }

                                if (sender.IsFacing(ally) && Prediction.Position.PredictUnitPosition(ally, 250).IsInRange(sender, MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 0]))
                                {
                                    CastShield(ally);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

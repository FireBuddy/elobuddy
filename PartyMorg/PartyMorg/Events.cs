using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EvadePlus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static EvadePlus.EvadePlus evadePlus;

        static Events()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;

            var SkillshotDetector = new SkillshotDetector(DetectionTeam.EnemyTeam);
            evadePlus = new EvadePlus.EvadePlus(SkillshotDetector);
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
                        if (args.Target == ally)
                        {
                            if (args.SData.DisplayNameTranslated == "Disintegrate" && sender.HasBuff("pyromania_particle"))
                            {
                                CastShield(ally);
                            }
                            else if (args.SData.DisplayNameTranslated == "Dazzle" || args.SData.DisplayNameTranslated == "Aegis of Zeonia" || args.SData.DisplayNameTranslated == "Heroic Charge" || args.SData.DisplayNameTranslated == "Pick A Card" || args.SData.DisplayNameTranslated == "Aethereal Chains" || args.SData.DisplayNameTranslated == "Mimic: Aethereal Chains" || args.SData.DisplayNameTranslated == "Rune Prison" || args.SData.DisplayNameTranslated == "Equilibrium Strike" || args.SData.DisplayNameTranslated == "Reckoning" || args.SData.DisplayNameTranslated == "Seismic Shard" || args.SData.DisplayNameTranslated == "Wither" || args.SData.DisplayNameTranslated == "Ice Blast" || args.SData.DisplayNameTranslated == "Two-Shiv Poison" || args.SData.DisplayNameTranslated == "Fling" || args.SData.DisplayNameTranslated == "Buster Shot" || args.SData.DisplayNameTranslated == "Chomp" || args.SData.DisplayNameTranslated == "Mocking Shout" || args.SData.DisplayNameTranslated == "Hyper-Kinetic Position Reverser" || args.SData.DisplayNameTranslated == "Three Talon Strike" || args.SData.DisplayNameTranslated == "Audacious Charge" || args.SData.DisplayNameTranslated == "Time Warp" || args.SData.DisplayNameTranslated == "Puncturing Taunt" || args.SData.DisplayNameTranslated == "Terrify" || args.SData.DisplayNameTranslated == "Null Sphere" || args.SData.DisplayNameTranslated == "Nether Grasp" || args.SData.DisplayNameTranslated == "Infinite Duress" || args.SData.DisplayNameTranslated == "Zephyr")
                            {
                                CastShield(ally);
                            }
                        }
                        else
                        {
                            foreach (var shieldThisSpell in AutoShield.ShieldSpellList.Where(x => x.DisplayName.Contains(args.SData.DisplayNameTranslated) && x.CurrentValue))
                            {
                                var allyPath = Prediction.Position.GetRealPath(ally);

                                if (evadePlus.IsHeroInDanger(ally) && !evadePlus.IsPathSafe(allyPath))
                                {
                                    if (args.SData.Name == "DariusAxeGrabCone" || args.SData.Name == "Volley" || args.SData.Name == "CassiopeiaPetrifyingGaze" || args.SData.Name == "FeralScream")
                                    {
                                        if (sender.IsFacing(ally))
                                        {
                                            CastShield(ally);
                                        }
                                    }
                                    else
                                    {
                                        CastShield(ally);
                                    }
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
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                    {
                        if (args.Target == ally)
                        {
                            if (args.SData.DisplayNameTranslated == "Disintegrate" && sender.HasBuff("pyromania_particle"))
                            {
                                CastShield(ally);
                            }
                            else if (args.SData.DisplayNameTranslated == "Dazzle" || args.SData.DisplayNameTranslated == "Aegis of Zeonia" || args.SData.DisplayNameTranslated == "Heroic Charge" || args.SData.DisplayNameTranslated == "Pick A Card" || args.SData.DisplayNameTranslated == "Aethereal Chains" || args.SData.DisplayNameTranslated == "Mimic: Aethereal Chains" || args.SData.DisplayNameTranslated == "Rune Prison" || args.SData.DisplayNameTranslated == "Equilibrium Strike" || args.SData.DisplayNameTranslated == "Reckoning" || args.SData.DisplayNameTranslated == "Seismic Shard" || args.SData.DisplayNameTranslated == "Wither" || args.SData.DisplayNameTranslated == "Ice Blast" || args.SData.DisplayNameTranslated == "Two-Shiv Poison" || args.SData.DisplayNameTranslated == "Fling" || args.SData.DisplayNameTranslated == "Buster Shot" || args.SData.DisplayNameTranslated == "Chomp" || args.SData.DisplayNameTranslated == "Mocking Shout" || args.SData.DisplayNameTranslated == "Hyper-Kinetic Position Reverser" || args.SData.DisplayNameTranslated == "Three Talon Strike" || args.SData.DisplayNameTranslated == "Audacious Charge" || args.SData.DisplayNameTranslated == "Time Warp" || args.SData.DisplayNameTranslated == "Puncturing Taunt" || args.SData.DisplayNameTranslated == "Terrify" || args.SData.DisplayNameTranslated == "Null Sphere" || args.SData.DisplayNameTranslated == "Nether Grasp" || args.SData.DisplayNameTranslated == "Infinite Duress" || args.SData.DisplayNameTranslated == "Zephyr")
                            {
                                CastShield(ally);
                            }
                        }
                        else
                        {
                            foreach (var shieldThisSpell in AutoShield.ShieldSpellList.Where(x => x.DisplayName.Contains(args.SData.DisplayNameTranslated) && x.CurrentValue))
                            {
                                var allyPath = Prediction.Position.GetRealPath(ally);

                                if (evadePlus.IsHeroInDanger(ally) && !evadePlus.IsPathSafe(allyPath))
                                {
                                    if (args.SData.Name == "DariusAxeGrabCone" || args.SData.Name == "Volley" || args.SData.Name == "CassiopeiaPetrifyingGaze" || args.SData.Name == "FeralScream")
                                    {
                                        if (sender.IsFacing(ally))
                                        {
                                            CastShield(ally);
                                        }
                                    }
                                    else
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
}

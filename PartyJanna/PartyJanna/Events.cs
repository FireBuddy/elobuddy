using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EvadePlus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _Interrupter = PartyJanna.Config.Settings.Interrupter;
using AntiGapcloser = PartyJanna.Config.Settings.AntiGapcloser;
using AutoShield = PartyJanna.Config.Settings.AutoShield;
using Humanizer = PartyJanna.Config.Settings.Humanizer;

namespace PartyJanna
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
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
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

        private static void OnInterruptableSpell(Obj_AI_Base sender, EloBuddy.SDK.Events.Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy || Player.Instance.IsRecalling())
            { return; }

            if (e.DangerLevel == DangerLevel.High)
            {
                if (_Interrupter.RInterruptDangerous && SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender) && Player.Instance.Mana >= 100)
                {
                    if (Humanizer.RCastDelayEnabled)
                    {
                        if (Humanizer.RRndmDelay)
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.RCastDelay))
                            {
                                SpellManager.R.Cast();
                                stopwatch.Reset();
                            }
                        }
                        else
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= Humanizer.RCastDelay)
                            {
                                SpellManager.R.Cast();
                                stopwatch.Reset();
                            }
                        }
                    }
                    else
                    {
                        SpellManager.R.Cast();
                    }
                }
                else
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
                                    SpellManager.Q.Cast();
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                                {
                                    SpellManager.Q.Cast();
                                    stopwatch.Reset();
                                }
                            }
                        }
                        else
                        {
                            SpellManager.Q.Cast();
                        }
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
                                SpellManager.Q.Cast();
                                stopwatch.Reset();
                            }
                        }
                        else
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                            {
                                SpellManager.Q.Cast();
                                stopwatch.Reset();
                            }
                        }
                    }
                    else
                    {
                        SpellManager.Q.Cast();
                    }
                }
            }
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsRecalling())
            { return; }

            priorAllyOrder = new List<AIHeroClient>();

            hpAllyOrder = new List<AIHeroClient>();

            highestPriority = 0;

            lowestHP = int.MaxValue;

            if (sender.IsEnemy && sender.IsMinion)
            {
                foreach (var ally in EntityManager.Heroes.Allies.Where(ally => ally.CountEnemiesInRange(1000) == 0))
                {
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                    {
                        if (args.Target == ally)
                        {
                            CastShield(sender);
                        }
                    }
                }

                if (AutoShield.TurretShieldMinion)
                {
                    foreach (var turret in EntityManager.Turrets.Allies)
                    {
                        if (args.Target == turret && Player.Instance.IsInRange(turret, SpellManager.E.Range))
                        {
                            CastShield(turret);
                        }
                    }
                }
            }

            if (sender.IsAlly && sender.IsRanged && !sender.IsMinion && AutoShield.BoostAD)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies)
                {
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(sender.Name) && x.CurrentValue))
                    {
                        if (args.Target == enemy)
                        {
                            CastShield(sender);
                        }
                    }
                }
            }

            if (sender.IsEnemy)
            {
                if (!sender.IsMinion)
                {
                    if (AutoShield.PriorMode == 0)
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
                                    CastShield(ally);
                                }
                            }
                        }
                    }
                    else
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
                                    CastShield(ally);
                                }
                            }
                        }
                    }

                    if (AutoShield.TurretShieldChampion)
                    {
                        foreach (var turret in EntityManager.Turrets.Allies)
                        {
                            if (args.Target == turret && Player.Instance.IsInRange(turret, SpellManager.E.Range))
                            {
                                CastShield(turret);
                            }
                        }
                    }
                }
            }

            if (AutoShield.SelfShield)
            {
                if (args.Target != null && args.Target.IsMe)
                {
                    CastShield(Player.Instance);
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
                            CastShield(ally);
                        }
                        else
                        {
                            var AllyPath = Prediction.Position.GetRealPath(ally);

                            if (evadePlus.IsHeroInDanger(ally) && !evadePlus.IsPathSafe(AllyPath))
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
                            CastShield(ally);
                        }
                        else
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

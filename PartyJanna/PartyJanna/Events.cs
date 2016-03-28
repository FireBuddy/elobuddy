using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using AutoShield = PartyJanna.Config.Settings.AutoShield;
using AntiGapcloser = PartyJanna.Config.Settings.AntiGapcloser;
using Interrupter = PartyJanna.Config.Settings.Interrupter;
using Humanizer = PartyJanna.Config.Settings.Humanizer;
using System;

namespace PartyJanna
{
    public static class Events
    {
        public static List<AIHeroClient> PriorAllyOrder { get; private set; }
        public static List<AIHeroClient> HpAllyOrder { get; private set; }
        public static int HighestPriority { get; private set; }
        public static float LowestHP { get; private set; }
        public static Stopwatch stopwatch = new Stopwatch();

        static Events()
        {
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            EloBuddy.SDK.Events.Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        public static void Initialize() { }

        public static bool PathIsInSpellRange(Vector3[] pathway, GameObjectProcessSpellCastEventArgs spell, float spellrange)
        {
            if (pathway != null)
            {
                foreach (var v in pathway)
                {
                    if (v.IsInRange(spell.End, spellrange))
                    { return true; }
                }
            }

            return false;
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy || !AntiGapcloser.AntiGap)
            { return; }

            foreach (var ally in EntityManager.Heroes.Allies)
            {
                if (e.End.Distance(ally) <= 300 && SpellManager.Q.IsInRange(sender.Position))
                {
                    SpellManager.Q.Cast(sender.Position);
                }
            }
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, EloBuddy.SDK.Events.Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy)
            { return; }

            if (e.DangerLevel == DangerLevel.High)
            {
                if (Interrupter.RInterruptDangerous && SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender) && Player.Instance.Mana >= 100)
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
                    if (Interrupter.QInterruptDangerous && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
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
                }
            }
            else
            {
                if (Interrupter.QInterrupt && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
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
            }
        }

        public static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            if (sender.IsEnemy && sender.IsMinion)
            {
                foreach (var ally in EntityManager.Heroes.Allies.Where(ally => ally.CountEnemiesInRange(1000) == 0))
                {
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                    {
                        if (args.Target == ally)
                        {
                            if (Humanizer.ERndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                {
                                    SpellManager.E.Cast(sender);
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                {
                                    SpellManager.E.Cast(sender);
                                    stopwatch.Reset();
                                }
                            }
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
                            if (Humanizer.ERndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                {
                                    SpellManager.E.Cast(sender);
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                {
                                    SpellManager.E.Cast(sender);
                                    stopwatch.Reset();
                                }
                            }
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
                            if (ally.Health <= LowestHP)
                            {
                                LowestHP = ally.Health;
                                HpAllyOrder.Insert(0, ally);
                            }
                            else
                            {
                                HpAllyOrder.Add(ally);
                            }
                        }

                        foreach (var ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                        {
                            foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (args.Target == ally)
                                {
                                    if (Humanizer.ERndmDelay)
                                    {
                                        stopwatch.Start();

                                        if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                        {
                                            SpellManager.E.Cast(ally);
                                            stopwatch.Reset();
                                        }
                                    }
                                    else
                                    {
                                        stopwatch.Start();

                                        if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                        {
                                            SpellManager.E.Cast(ally);
                                            stopwatch.Reset();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var slider in AutoShield.Sliders)
                        {
                            if (slider.CurrentValue >= HighestPriority)
                            {
                                HighestPriority = slider.CurrentValue;

                                foreach (var ally in AutoShield.Heros)
                                {
                                    if (slider.VisibleName.Contains(ally.ChampionName))
                                    {
                                        PriorAllyOrder.Insert(0, ally);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var ally in AutoShield.Heros)
                                {
                                    if (slider.VisibleName.Contains(ally.ChampionName))
                                    {
                                        PriorAllyOrder.Add(ally);
                                    }
                                }
                            }
                        }

                        foreach (var ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                        {
                            foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (args.Target == ally)
                                {
                                    if (Humanizer.ERndmDelay)
                                    {
                                        stopwatch.Start();

                                        if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                        {
                                            SpellManager.E.Cast(ally);
                                            stopwatch.Reset();
                                        }
                                    }
                                    else
                                    {
                                        stopwatch.Start();

                                        if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                        {
                                            SpellManager.E.Cast(ally);
                                            stopwatch.Reset();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (AutoShield.TurretShield)
                {
                    foreach (var turret in EntityManager.Turrets.Allies)
                    {
                        if (args.Target == turret && Player.Instance.IsInRange(turret, SpellManager.E.Range))
                        {
                            if (Humanizer.ERndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                {
                                    SpellManager.E.Cast(turret);
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                {
                                    SpellManager.E.Cast(turret);
                                    stopwatch.Reset();
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy)
            { return; }

            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            if (AutoShield.PriorMode == 1)
            {
                foreach (var slider in AutoShield.Sliders)
                {
                    if (slider.CurrentValue >= HighestPriority)
                    {
                        HighestPriority = slider.CurrentValue;

                        foreach (var ally in AutoShield.Heros)
                        {
                            if (slider.VisibleName.Contains(ally.ChampionName))
                            {
                                PriorAllyOrder.Insert(0, ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (var ally in AutoShield.Heros)
                        {
                            if (slider.VisibleName.Contains(ally.ChampionName))
                            {
                                PriorAllyOrder.Add(ally);
                            }
                        }
                    }
                }

                foreach (var ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                    {
                        if (args.Target != null && args.Target == ally)
                        {
                            if (Humanizer.ERndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
                                }
                            }
                        }

                        if (ally.IsInRange(args.End, 200) || PathIsInSpellRange(ally.RealPath(), args, 200))
                        {
                            if (Humanizer.ERndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
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
                    if (ally.Health <= LowestHP)
                    {
                        LowestHP = ally.Health;
                        HpAllyOrder.Insert(0, ally);
                    }
                    else
                    {
                        HpAllyOrder.Add(ally);
                    }
                }

                foreach (var ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    foreach (var shieldThisAlly in AutoShield.ShieldAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                    {
                        if (args.Target != null && args.Target == ally)
                        {
                            if (Humanizer.ERndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
                                }
                            }
                        }

                        if (ally.IsInRange(args.End, args.SData.CastRadius) || PathIsInSpellRange(ally.RealPath(), args, 200))
                        {
                            if (Humanizer.ERndmDelay)
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
                                }
                            }
                            else
                            {
                                stopwatch.Start();

                                if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                                {
                                    SpellManager.E.Cast(ally);
                                    stopwatch.Reset();
                                }
                            }
                        }
                    }
                }
            }

            if (AutoShield.SelfShield)
            {
                if (args.Target != null && args.Target.IsMe)
                {
                    if (Humanizer.ERndmDelay)
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                        {
                            SpellManager.E.Cast(Player.Instance);
                            stopwatch.Reset();
                        }
                    }
                    else
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                        {
                            SpellManager.E.Cast(Player.Instance);
                            stopwatch.Reset();
                        }
                    }
                }

                if (Player.Instance.IsInRange(args.End, args.SData.CastRadius) || PathIsInSpellRange(Player.Instance.RealPath(), args, 200))
                {
                    if (Humanizer.ERndmDelay)
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.ECastDelay))
                        {
                            SpellManager.E.Cast(Player.Instance);
                            stopwatch.Reset();
                        }
                    }
                    else
                    {
                        stopwatch.Start();

                        if (stopwatch.ElapsedMilliseconds >= Humanizer.ECastDelay)
                        {
                            SpellManager.E.Cast(Player.Instance);
                            stopwatch.Reset();
                        }
                    }
                }
            }
        }
    }
}

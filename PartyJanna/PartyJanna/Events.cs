using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AntiGapcloser = PartyJanna.Config.Settings.AntiGapcloser;
using AutoShield = PartyJanna.Config.Settings.AutoShield;
using Humanizer = PartyJanna.Config.Settings.Humanizer;
using Interrupter = PartyJanna.Config.Settings.Interrupter;

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
            if (!sender.IsEnemy || !AntiGapcloser.AntiGap || Player.Instance.IsRecalling())
            { return; }

            foreach (var ally in EntityManager.Heroes.Allies)
            {
                if (e.End.Distance(ally) <= 300 && SpellManager.Q.IsInRange(sender.Position))
                {
                    if (Humanizer.QCastDelayEnabled)
                    {
                        if (Humanizer.QRndmDelay)
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Humanizer.QCastDelay))
                            {
                                SpellManager.Q.Cast(sender.Position);
                                stopwatch.Reset();
                            }
                        }
                        else
                        {
                            stopwatch.Start();

                            if (stopwatch.ElapsedMilliseconds >= Humanizer.QCastDelay)
                            {
                                SpellManager.Q.Cast(sender.Position);
                                stopwatch.Reset();
                            }
                        }
                    }
                    else
                    {
                        SpellManager.Q.Cast(sender.Position);
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
                if (Interrupter.RInterruptDangerous && SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender) && Player.Instance.Mana >= 100)
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
                    if (Interrupter.QInterruptDangerous && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
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
                if (Interrupter.QInterrupt && SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
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
                            if (Humanizer.ECastDelayEnabled)
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
                            else
                            {
                                SpellManager.E.Cast(sender);
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
                            if (Humanizer.ECastDelayEnabled)
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
                            else
                            {
                                SpellManager.E.Cast(sender);
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
                                    if (Humanizer.ECastDelayEnabled)
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
                                    else
                                    {
                                        SpellManager.E.Cast(ally);
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
                                    if (Humanizer.ECastDelayEnabled)
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
                                    else
                                    {
                                        SpellManager.E.Cast(ally);
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
                            if (Humanizer.ECastDelayEnabled)
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
                            else
                            {
                                SpellManager.E.Cast(turret);
                            }
                        }
                    }
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || Player.Instance.IsRecalling())
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
                            if (Humanizer.ECastDelayEnabled)
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
                            else
                            {
                                SpellManager.E.Cast(ally);
                            }
                        }

                        if (ally.IsInRange(args.End, 200) || PathIsInSpellRange(ally.RealPath(), args, 200))
                        {
                            if (Humanizer.ECastDelayEnabled)
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
                            else
                            {
                                SpellManager.E.Cast(ally);
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
                            if (Humanizer.ECastDelayEnabled)
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
                            else
                            {
                                SpellManager.E.Cast(ally);
                            }
                        }

                        if (ally.IsInRange(args.End, args.SData.CastRadius) || PathIsInSpellRange(ally.RealPath(), args, 200))
                        {
                            if (Humanizer.ECastDelayEnabled)
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
                            else
                            {
                                SpellManager.E.Cast(ally);
                            }
                        }
                    }
                }
            }

            if (AutoShield.SelfShield)
            {
                if (args.Target != null && args.Target.IsMe)
                {
                    if (Humanizer.ECastDelayEnabled)
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
                    else
                    {
                        SpellManager.E.Cast(Player.Instance);
                    }
                }

                if (Player.Instance.IsInRange(args.End, args.SData.CastRadius) || PathIsInSpellRange(Player.Instance.RealPath(), args, 200))
                {
                    if (Humanizer.ECastDelayEnabled)
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
                    else
                    {
                        SpellManager.E.Cast(Player.Instance);
                    }
                }
            }
        }
    }
}

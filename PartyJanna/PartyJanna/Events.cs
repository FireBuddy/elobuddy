using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK.Menu.Values;
using Settings = PartyJanna.Config.Modes.AutoShield;
using EloBuddy.SDK.Events;
using System;

namespace PartyJanna
{
    public static class Events
    {
        public static List<AIHeroClient> PriorAllyOrder { get; private set; }
        public static List<AIHeroClient> HpAllyOrder { get; private set; }
        public static int HighestPriority { get; private set; }
        public static float LowestHP { get; private set; }

        static Events()
        {
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
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
            if (!sender.IsEnemy)
            { return; }

            foreach (var ally in EntityManager.Heroes.Allies)
            {
                if (e.End.Distance(ally) <= 300 && SpellManager.Q.IsInRange(sender))
                {
                    SpellManager.Q.Cast(sender);
                }
            }
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy)
            { return; }

            if (e.DangerLevel == DangerLevel.High)
            {
                if (SpellManager.R.IsReady() && SpellManager.R.IsInRange(sender))
                {
                    SpellManager.R.Cast();
                }
                else
                {
                    if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
                    {
                        SpellManager.Q.Cast(sender);
                    }
                }
            }
            else
            {
                if (SpellManager.Q.IsReady() && SpellManager.Q.IsInRange(sender))
                {
                    SpellManager.Q.Cast(sender);
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
                foreach (var Ally in EntityManager.Heroes.Allies.Where(Ally => Ally.CountEnemiesInRange(1000) == 0))
                {
                    if (args.Target == Ally)
                    {
                        SpellManager.E.Cast(sender);
                    }
                }
            }

            if (sender.IsAlly && sender.IsRanged && !sender.IsMinion && Settings.BoostAD)
            {
                foreach (var Enemy in EntityManager.Heroes.Enemies)
                {
                    if (args.Target == Enemy)
                    {
                        SpellManager.E.Cast(sender);
                    }
                }
            }

            if (sender.IsEnemy)
            {
                if (!sender.IsMinion)
                {
                    if (Settings.PriorMode == 0)
                    {
                        foreach (var Ally in EntityManager.Heroes.Allies)
                        {
                            if (Ally.Health <= LowestHP)
                            {
                                LowestHP = Ally.Health;
                                HpAllyOrder.Insert(0, Ally);
                            }
                            else
                            {
                                HpAllyOrder.Add(Ally);
                            }
                        }

                        foreach (var Ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                        {
                            if (args.Target == Ally)
                            {
                                SpellManager.E.Cast(Ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (var Slider in Settings.Sliders)
                        {
                            if (Slider.CurrentValue >= HighestPriority)
                            {
                                HighestPriority = Slider.CurrentValue;

                                foreach (var Ally in Settings.Heros)
                                {
                                    if (Slider.VisibleName.Contains(Ally.ChampionName))
                                    {
                                        PriorAllyOrder.Insert(0, Ally);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var Ally in Settings.Heros)
                                {
                                    if (Slider.VisibleName.Contains(Ally.ChampionName))
                                    {
                                        PriorAllyOrder.Add(Ally);
                                    }
                                }
                            }
                        }

                        foreach (var Ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                        {
                            if (args.Target == Ally)
                            {
                                SpellManager.E.Cast(Ally);
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

            if (Settings.PriorMode == 1)
            {
                foreach (var Slider in Settings.Sliders)
                {
                    if (Slider.CurrentValue >= HighestPriority)
                    {
                        HighestPriority = Slider.CurrentValue;

                        foreach (var Ally in Settings.Heros)
                        {
                            if (Slider.VisibleName.Contains(Ally.ChampionName))
                            {
                                PriorAllyOrder.Insert(0, Ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (var Ally in Settings.Heros)
                        {
                            if (Slider.VisibleName.Contains(Ally.ChampionName))
                            {
                                PriorAllyOrder.Add(Ally);
                            }
                        }
                    }
                }

                foreach (var Ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    if (Ally.IsInRange(args.End, 200) || PathIsInSpellRange(Ally.RealPath(), args, 200))
                    {
                        SpellManager.E.Cast(Ally);
                    }
                }

            }
            else
            {
                foreach (var Ally in EntityManager.Heroes.Allies)
                {
                    if (Ally.Health <= LowestHP)
                    {
                        LowestHP = Ally.Health;
                        HpAllyOrder.Insert(0, Ally);
                    }
                    else
                    {
                        HpAllyOrder.Add(Ally);
                    }
                }

                foreach (var Ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, SpellManager.E.Range)))
                {
                    if (Ally.IsInRange(args.End, args.SData.CastRadius) || PathIsInSpellRange(Ally.RealPath(), args, 200))
                    {
                        SpellManager.E.Cast(Ally);
                    }
                }
            }
        }
    }
}

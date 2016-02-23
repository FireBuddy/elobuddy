using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;
using System.Linq;

namespace PartyJanna
{
    public static class AutoShield
    {
        private static List<AIHeroClient> PriorAllyOrder { get; set; }
        private static List<AIHeroClient> HpAllyOrder { get; set; }

        private static int HighestPriority { get; set; }

        private static float LowestHP { get; set; }

        public static void Init()
        { }

        static AutoShield()
        {
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }

        public static void Execute()
        {
            /*if (Config.Combo.UseE.CurrentValue && Config.Spells.E.IsReady() && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
            {
                if (Config.AutoShield.PriorityMode.SelectedIndex == 0)
                {
                    foreach (AIHeroClient Ally in EntityManager.Heroes.Allies)
                    {
                        if (Ally.ChampionName != Config.AddonChampion && Ally.IsInRange(Player.Instance, Config.Spells.E.Range))
                        {
                            foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
                            {
                                if (Ally.IsInAutoAttackRange(Enemy))
                                {
                                    Config.Spells.E.Cast(Ally);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (AIHeroClient Ally in HpAllyOrder)
                    {
                        if (Ally.ChampionName != Config.AddonChampion && Ally.IsInRange(Player.Instance, Config.Spells.E.Range))
                        {
                            foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
                            {
                                if (Ally.IsInAutoAttackRange(Enemy))
                                {
                                    Config.Spells.E.Cast(Ally);
                                }
                            }
                        }
                    }
                }
        }*/
        }

        private static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.Mana < Config.Spells.manaE[Config.Spells.E.Level] || !Config.AutoShield.UseE.CurrentValue)
            { return; }

            Startup.CurrentFunction = "AutoShield";

            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            /*if (sender.IsAlly && sender.IsRanged && args.Target.IsEnemy && !args.Target)
            {
                Config.Spells.E.Cast(sender);
            }*/

            if (sender.IsEnemy)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    if (args.Target.IsMe)
                    {
                        Config.Spells.E.Cast(Player.Instance);
                    }
                }

                if (Config.AutoShield.PriorityMode.SelectedIndex == 0)
                {
                    foreach (AIHeroClient Ally in EntityManager.Heroes.Allies)
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

                    foreach (AIHeroClient Ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, Config.Spells.E.Range)))
                    {
                        if (args.Target == Ally)
                        {
                            Config.Spells.E.Cast(Ally);
                        }
                    }
                }
                else
                {
                    foreach (Slider Slider in Config.AutoShield.SliderList)
                    {
                        if (Slider.CurrentValue >= HighestPriority)
                        {
                            HighestPriority = Slider.CurrentValue;

                            foreach (AIHeroClient Ally in Config.AutoShield.AIHeroClientList)
                            {
                                if (Slider.VisibleName.Contains(Ally.ChampionName))
                                {
                                    PriorAllyOrder.Insert(0, Ally);
                                }
                            }
                        }
                        else
                        {
                            foreach (AIHeroClient Ally in Config.AutoShield.AIHeroClientList)
                            {
                                if (Slider.VisibleName.Contains(Ally.ChampionName))
                                {
                                    PriorAllyOrder.Add(Ally);
                                }
                            }
                        }
                    }

                    foreach (AIHeroClient Ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, Config.Spells.E.Range)))
                    {
                        if (args.Target == Ally)
                        {
                            Config.Spells.E.Cast(Ally);
                        }
                    }
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || Player.Instance.Mana < Config.Spells.manaE[Config.Spells.E.Level] || !Config.AutoShield.UseE.CurrentValue)
            { return; }

            Startup.CurrentFunction = "AutoShield";

            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (args.Target.IsMe)
                {
                    Config.Spells.E.Cast(Player.Instance);
                }
            }

            if (Config.AutoShield.PriorityMode.SelectedIndex == 1)
            {
                foreach (Slider Slider in Config.AutoShield.SliderList)
                {
                    if (Slider.CurrentValue >= HighestPriority)
                    {
                        HighestPriority = Slider.CurrentValue;

                        foreach (AIHeroClient Ally in Config.AutoShield.AIHeroClientList)
                        {
                            if (Slider.VisibleName.Contains(Ally.ChampionName))
                            {
                                PriorAllyOrder.Insert(0, Ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (AIHeroClient Ally in Config.AutoShield.AIHeroClientList)
                        {
                            if (Slider.VisibleName.Contains(Ally.ChampionName))
                            {
                                PriorAllyOrder.Add(Ally);
                            }
                        }
                    }
                }

                    foreach (AIHeroClient Ally in PriorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, Config.Spells.E.Range)))
                    {
                        if (Ally.IsInRange(args.End, args.SData.CastRadius) /*args.End.Distance(Ally) <= args.SData.CastRadius*/)
                        {
                            Config.Spells.E.Cast(Ally);
                        }
                    }
                
            }
            else
            {
                foreach (AIHeroClient Ally in EntityManager.Heroes.Allies)
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

                foreach (AIHeroClient Ally in HpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, Config.Spells.E.Range)))
                {
                    if (Ally.IsInRange(args.End, args.SData.CastRadius) /*args.End.Distance(Ally) <= args.SData.CastRadius*/)
                    {
                        Config.Spells.E.Cast(Ally);
                    }
                }
            }
        }
    }
}

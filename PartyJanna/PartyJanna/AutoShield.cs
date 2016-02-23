using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;
using System.Linq;

namespace PartyJanna
{
    public static class AutoShield
    {
        public static void Init()
        { }

        private static List<AIHeroClient> PriorAllyOrder { get; set; }
        private static List<AIHeroClient> HpAllyOrder { get; set; }

        private static int HighestPriority { get; set; }

        private static float LowestHP { get; set; }

        private static Prediction.Position.PredictionData PredictionData { get; set; }

        private static bool IgnoreMinionCollision { get; set; }

        private static AIHeroClient GetTarget { get; set; }

        public static void Execute()
        {
            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            if (Config.Combo.UseE.CurrentValue && Config.Spells.E.IsReady() && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
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

                if (Config.AutoShield.PriorityMode.CurrentValue == 0)
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

                                if (Enemy.Spellbook.SpellWasCast && Ally.IsInRange(Enemy, Enemy.CastRange))
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

                                if (Enemy.Spellbook.SpellWasCast && Ally.IsInRange(Enemy, Enemy.CastRange))
                                {
                                    Config.Spells.E.Cast(Ally);
                                }
                            }
                        }
                    }
                }
            }
        }

        static AutoShield()
        {
            Startup.CurrentFunction = "AutoShield";

            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.Mana < Config.Spells.manaE[Config.Spells.E.Level])
            { return; }

            if (sender.IsEnemy)
            {
                foreach (AIHeroClient Ally in EntityManager.Heroes.Allies.Where(ally => Player.Instance.IsInRange(ally, Config.Spells.E.Range)))
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
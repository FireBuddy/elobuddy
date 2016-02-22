using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;

namespace PartyJanna.Functions
{
    public static class Combo
    {
        private static List<AIHeroClient> PriorAllyOrder { get; set; }
        private static List<AIHeroClient> HpAllyOrder { get; set; }

        private static int HighestPriority { get; set; }

        private static float LowestHP { get; set; }

        private static Prediction.Position.PredictionData PredictionData { get; set; }

        private static bool IgnoreMinionCollision { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Combo";

            PriorAllyOrder = new List<AIHeroClient>();

            HpAllyOrder = new List<AIHeroClient>();

            PredictionData = new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed);

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            TargetSelector.GetTarget(Config.Spells.Q.Range, DamageType.Mixed);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (Slider Slider in Config.Protect.SliderList)
                {
                    if (Slider.CurrentValue >= HighestPriority)
                    {
                        HighestPriority = Slider.CurrentValue;

                        foreach (AIHeroClient Ally in Config.Protect.AIHeroClientList)
                        {
                            if (Slider.VisibleName.Contains(Ally.ChampionName))
                            {
                                PriorAllyOrder.Insert(0, Ally);
                            }
                        }
                    }
                    else
                    {
                        foreach (AIHeroClient Ally in Config.Protect.AIHeroClientList)
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

                if (TargetSelector.SelectedTarget.IsValid && TargetSelector.SelectedTarget.IsEnemy && Config.Combo.UseQ.CurrentValue && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level] && TargetSelector.SelectedTarget.IsInRange(Player.Instance, Config.Spells.Q.Range))
                {
                    if (Player.Instance.CountEnemiesInRange(Config.Spells.Q.Range + 525) <= 2)
                    {
                        IgnoreMinionCollision = false;
                    }
                    else
                    {
                        IgnoreMinionCollision = true;
                    }

                    Config.Spells.Q.Cast(Prediction.Position.GetPrediction(TargetSelector.SelectedTarget, PredictionData, IgnoreMinionCollision).CastPosition);
                }

                if (Config.Combo.UseE.CurrentValue && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
                {
                    if (Config.Protect.PriorityMode.CurrentValue == 0)
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

                if (TargetSelector.SelectedTarget.IsValid && TargetSelector.SelectedTarget.IsEnemy && Config.Combo.UseW.CurrentValue && Player.Instance.Mana >= Config.Spells.manaW[Config.Spells.W.Level] && TargetSelector.SelectedTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(TargetSelector.SelectedTarget);
                }
            }
        }
    }
}


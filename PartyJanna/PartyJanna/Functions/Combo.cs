using EloBuddy.SDK;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;
using System;
using SharpDX;

namespace PartyJanna.Functions
{
    public static class Combo
    {
        private static List<string> PriorityOrder { get; set; }

        private static int HighestPriority { get; set; }

        private static Prediction.Position.PredictionData PredictionData { get; set; }

        private static bool IgnoreMinionCollision { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Combo";
            
            if (Config.MyHero.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    PriorityOrder = new List<string>();

                    PredictionData = new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Radius, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed);

                    HighestPriority = 0;

                    foreach (Slider PrioritySlider in Config.Protect.PrioritySliderList)
                    {
                        if (PrioritySlider.CurrentValue >= HighestPriority)
                        {
                            HighestPriority = PrioritySlider.CurrentValue;
                            PriorityOrder.Insert(0, PrioritySlider.VisibleName);
                        }
                        else
                        {
                            PriorityOrder.Add(PrioritySlider.VisibleName);
                        }
                    }

                    foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
                    {
                        foreach (AIHeroClient Ally in EntityManager.Heroes.Allies)
                        {
                            if (Enemy.IsInRange(Config.MyHero, Config.Spells.Q.Range))
                            {
                                if (Config.MyHero.CountEnemiesInRange(Config.Spells.Q.Range) <= 2)
                                {
                                    IgnoreMinionCollision = false;
                                }
                                else
                                {
                                    IgnoreMinionCollision = true;
                                }

                                Config.Spells.Q.Cast(Prediction.Position.PredictCircularMissile(Enemy, Config.Spells.Q.Range, Config.Spells.Q.Width, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed, null, IgnoreMinionCollision).CastPosition);
                            }

                            if (Enemy.IsInRange(Config.MyHero, Config.Spells.W.Range))
                            {
                                Config.Spells.W.Cast(Enemy);
                            }

                            if (Ally.ChampionName != Config.AddonChampion && Ally.IsInRange(Config.MyHero, Config.Spells.E.Range))
                            {
                                if (Ally.IsInAutoAttackRange(Enemy) || Enemy.Spellbook.SpellWasCast && Ally.IsInRange(Enemy, Enemy.CastRange))
                                {
                                    Config.Spells.E.Cast(Ally);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

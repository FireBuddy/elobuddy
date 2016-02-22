using EloBuddy.SDK;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;
using System;

namespace PartyJanna.Functions
{
    public static class Combo
    {
        public static List<string> PriorityOrder;

        private static int HighestPriority = 0;

        public static void Execute()
        {
            Startup.CurrentFunction = "Combo";

            if (Config.MyHero.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    PriorityOrder = new List<string>();

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
                        if (Enemy.IsInRange(Config.MyHero, Config.Spells.Q.Range))
                        {
                            Config.Spells.Q.Cast(Prediction.Position.GetPrediction(Enemy, new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Radius, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed), true).CastPosition);
                        }

                        if (Enemy.IsInRange(Config.MyHero, Config.Spells.W.Range))
                        {
                            Config.Spells.W.Cast(Enemy);
                        }

                        foreach (AIHeroClient Ally in EntityManager.Heroes.Allies)
                        {
                            if (Ally.ChampionName != Config.AddonChampion)
                            {
                                if ((Ally.IsInAutoAttackRange(Enemy) || Ally.IsInRange(Enemy, Enemy.CastRange)) && Ally.IsInRange(Config.MyHero, Config.Spells.E.Range))
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

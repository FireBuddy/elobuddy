using EloBuddy.SDK;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace PartyJanna.Functions
{
    public static class Combo
    {
        public static List<string> PriorityOrder;

        private static int HighestPriority = 0;

        public static void Execute()
        {
            Startup.CurrentFunction = "Combo";

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
                    foreach (AIHeroClient Ally in EntityManager.Heroes.Allies)
                    {
                        if (Ally.ChampionName != Config.AddonChampion)
                        {
                            if ((Ally.IsInAutoAttackRange(Enemy) || Ally.IsInRange(Enemy, Enemy.CastRange)) && Ally.IsInRange(EntityManager.Heroes.Allies[0], Config.Spells.E.Range))
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

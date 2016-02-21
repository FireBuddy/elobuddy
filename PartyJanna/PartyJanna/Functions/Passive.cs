using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;

namespace PartyJanna.Functions
{
    public static class Passive
    {
        public static List<string> PriorityOrder = new List<string>();

        public static void Execute()
        {
            Startup.CurrentFunction = "Protect";

            int HighestPriority = 0;

            foreach (Slider PrioritySlider in Config.Protect.PrioritySliderList)
            {     
                if (PrioritySlider.CurrentValue >= HighestPriority)
                {
                    HighestPriority = PrioritySlider.CurrentValue;
                    PriorityOrder.Insert(0, PrioritySlider.DisplayName);
                }
                else
                {
                    PriorityOrder.Add(PrioritySlider.DisplayName);
                }
            }

            /*foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
            {
                if (Enemy.range)
            }*/
        }
    }
}

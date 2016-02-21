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

        private static int HighestPriority = 0;

        public static void Execute()
        {
            Startup.CurrentFunction = "Protect";

            foreach (Slider PrioritySlider in Config.Protect.PrioritySliderList)
            {
                PrioritySlider.DisplayName.Insert(0, PrioritySlider.CurrentValue.ToString());
            }

            PriorityOrder.Sort();
            PriorityOrder.Reverse();

            /*foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
            {
                if (Enemy.range)
            }*/
        }
    }
}

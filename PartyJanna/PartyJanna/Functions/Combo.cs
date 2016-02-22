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

                /*foreach (Slider PrioritySlider in Config.Protect.PrioritySliderList)
                {
                    PriorityOrder.Add(PrioritySlider.VisibleName.Insert(0, PrioritySlider.CurrentValue.ToString()));
                }

                PriorityOrder.Sort();
                PriorityOrder.Reverse();*/

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (var Element in PriorityOrder)
                {
                    Chat.Print(Element);
                }
            }
        }
    }
}

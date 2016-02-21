using EloBuddy.SDK;
using EloBuddy;

namespace PartyJanna.Functions
{
    public static class Combo
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "Combo";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                foreach (string str in Passive.PriorityOrder)
                {
                    Chat.Print(str);
                }

                Chat.Print("");
            }
        }
    }
}

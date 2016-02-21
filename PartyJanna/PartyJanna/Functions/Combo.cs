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
                Chat.Print(Passive.PriorityOrder);
            }
        }
    }
}

using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;
using PartyJanna;

namespace PartyJanna.Functions
{
    public static class Combo
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "Combo";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                
            }
        }
    }
}

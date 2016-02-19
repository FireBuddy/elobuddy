using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class Flee
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "Flee";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {

            }
        }
    }
}

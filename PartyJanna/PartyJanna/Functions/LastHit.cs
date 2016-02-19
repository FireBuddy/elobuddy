using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class LastHit
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "LastHit";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {

            }
        }
    }
}

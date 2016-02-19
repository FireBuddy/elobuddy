using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class LaneClear
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "LaneClear";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {

            }
        }
    }
}

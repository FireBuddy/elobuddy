using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class JungleClear
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "JungleClear";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {

            }
        }
    }
}

using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class LaneCleaner
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "LaneCleaner";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {

            }
        }
    }
}

using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class JungleCleaner
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "JungleCleaner";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {

            }
        }
    }
}

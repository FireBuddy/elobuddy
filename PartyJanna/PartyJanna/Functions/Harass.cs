using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class Harass
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "Harass";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {

            }
        }
    }
}

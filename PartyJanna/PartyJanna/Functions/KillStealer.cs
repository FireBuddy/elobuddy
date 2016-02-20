using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class KillStealer
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "KillStealer";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {

            }
        }
    }
}

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using System;
using System.Drawing;

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

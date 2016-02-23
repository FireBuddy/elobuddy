using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna
{
    public static class Interrupter
    {
        public static void Init()
        { }

        static Interrupter()
        {
            Startup.CurrentFunction = "Interrupter";
        }
    }
}
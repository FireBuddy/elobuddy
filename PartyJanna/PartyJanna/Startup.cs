using EloBuddy;
using EloBuddy.SDK.Events;
using PartyJanna.Functions;
using System;

namespace PartyJanna
{
    public static class Startup
    {
        public static string CurrentFunction { private get; set; }

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != Config.AddonChampion)
            {
                return;
            }

            Config.Init();
            //AutoShield.Init();
            Draw.Init();

            Chat.Print("{0} has finished loading.\nHold SHIFT to configure.", Config.AddonName);

            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            try
            {
                AutoShield.Execute();
                Combo.Execute();
                Harass.Execute();
                Flee.Execute();
                LaneCleaner.Execute();
            }
            catch (Exception e)
            {
                Chat.Print("Error - {0} function is not working correctly:\n{1}", CurrentFunction, e);
            }
        }
    }
}

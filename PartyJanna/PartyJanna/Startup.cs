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

            Chat.Print(string.Format("{0} has finished loading.\nHold SHIFT to configure.", Config.AddonName));

            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            try
            {
                Combo.Execute();
                Flee.Execute();
                Harass.Execute();
                JungleCleaner.Execute();
                LaneCleaner.Execute();
                KillStealer.Execute();
            }
            catch(Exception e)
            {
                Chat.Print(string.Format("Error - {0} function is not working correctly:\n{1}", CurrentFunction, e));
            }
        }
    }
}

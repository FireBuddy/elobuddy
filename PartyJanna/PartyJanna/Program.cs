using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace PartyJanna
{
    public static class Program
    {
        private const string ChampName = "Janna";

        public static void Main(string[] args) => Loading.OnLoadingComplete += OnLoadingComplete;

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != ChampName) return;

            Config.Initialize();
            SpellManager.Initialize();
            ModeManager.Initialize();
            Events.Initialize();

            Chat.Print("Welcome to PartyJanna, have fun!");

            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (Config.Settings.Draw.DrawQ)
                Circle.Draw(Color.White, SpellManager.Q.Range, Player.Instance.Position);

            if (Config.Settings.Draw.DrawW)
                Circle.Draw(Color.White, SpellManager.W.Range, Player.Instance.Position);

            if (Config.Settings.Draw.DrawE)
                Circle.Draw(Color.White, SpellManager.E.Range, Player.Instance.Position);

            if (Config.Settings.Draw.DrawR)
                Circle.Draw(Color.White, SpellManager.R.Range, Player.Instance.Position);
        }
    }
}
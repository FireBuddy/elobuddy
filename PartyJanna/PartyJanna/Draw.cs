using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;
using System;
using EloBuddy.SDK.Rendering;
using System.Drawing;

namespace PartyJanna
{
    public static class Draw
    {
        public static void Init()
        { }

        static Draw()
        {
            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            new Circle { Color = Color.WhiteSmoke, Radius = Config.Spells.Q.Range }.Draw(Player.Instance.Position);
            new Circle { Color = Color.WhiteSmoke, Radius = Config.Spells.W.Range }.Draw(Player.Instance.Position);
            new Circle { Color = Color.WhiteSmoke, Radius = Config.Spells.E.Range }.Draw(Player.Instance.Position);
            new Circle { Color = Color.WhiteSmoke, Radius = Config.Spells.R.Range }.Draw(Player.Instance.Position);
        }
    }
}

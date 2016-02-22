using EloBuddy;
using EloBuddy.SDK.Rendering;
using System;
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

        public static Color DrawingColor = Color.White;

        private static void OnDraw(EventArgs args)
        {
            if (Config.RangeCircles.DrawQ.CurrentValue)
            {
                new Circle { Color = DrawingColor, Radius = Config.Spells.Q.Range }.Draw(Player.Instance.Position);
            }

            if (Config.RangeCircles.DrawW.CurrentValue)
            {
                new Circle { Color = DrawingColor, Radius = Config.Spells.W.Range }.Draw(Player.Instance.Position);
            }

            if (Config.RangeCircles.DrawE.CurrentValue)
            {
                new Circle { Color = DrawingColor, Radius = Config.Spells.E.Range }.Draw(Player.Instance.Position);
            }

            if (Config.RangeCircles.DrawR.CurrentValue)
            {
                new Circle { Color = DrawingColor, Radius = Config.Spells.R.Range }.Draw(Player.Instance.Position);
            }
        }
    }
}

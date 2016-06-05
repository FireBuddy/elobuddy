using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace CustomItemBuyer
{
    internal static class Config
    {
        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu("CustomItemBuyer", "cib");
            Menu.AddGroupLabel("Welcome to CustomItemBuyer!");

            CIB.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class CIB
        {
            private static readonly Menu Menu;

            static CIB()
            {
                Menu = Config.Menu.AddSubMenu("Settings");

                BuyingOrderMenu.Initialize();

                Menu.AddSeparator();
            }

            public static void Initialize()
            {
            }

            public static class BuyingOrderMenu
            {
                public static readonly CheckBox rndmDelay;
                public static readonly CheckBox enabled;
                public static readonly CheckBox draw;
/*
                public static readonly CheckBox buyComp;
*/
                public static readonly Slider delay;

                static BuyingOrderMenu()
                {
                    Menu.AddGroupLabel("CIB Settings");

                    enabled = Menu.Add("active", new CheckBox("Enabled"));

                    Menu.AddSeparator(13);

                    draw = Menu.Add("draw", new CheckBox("Draw Text"));

                    Menu.AddSeparator(13);

                    //buyComp = Menu.Add("buycomp", new CheckBox("Buy Item Components"));

                    //Menu.AddSeparator(13);

                    delay = Menu.Add("delay", new Slider("Delay to Buy Each Item (1 sec = 1000 ms):", 500, 500, 2000));

                    Menu.AddSeparator(13);

                    rndmDelay = Menu.Add("rndmdelay", new CheckBox("Randomize Buy Delay"));
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}
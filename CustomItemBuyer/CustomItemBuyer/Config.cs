using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace CustomItemBuyer
{
    class Config
    {
        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu("CustomItemBuyer", "cib");
            Menu.AddGroupLabel("Welcome to CustomItemBuyer!");

            CIB.Initialize();
        }

        public static void Initialize() { }

        public static class CIB
        {
            private static readonly Menu Menu;

            static CIB()
            {
                Menu = Config.Menu.AddSubMenu("Settings");

                BuyingOrderMenu.Initialize();

                Menu.AddSeparator();
            }

            public static void Initialize() { }

            public static class BuyingOrderMenu
            {
                public static readonly CheckBox rndmDelay, enabled;
                public static readonly Slider delay;

                static BuyingOrderMenu()
                {
                    Menu.AddGroupLabel("CIB Settings");

                    enabled = Menu.Add("active", new CheckBox("Enabled?"));

                    Menu.AddSeparator(13);

                    delay = Menu.Add("delay", new Slider("Delay to Buy Items (1sec = 1000ms):", 500, 0, 2000));

                    Menu.AddSeparator(13);

                    rndmDelay = Menu.Add("rndmdelay", new CheckBox("Randomize Delay?"));
                }

                public static void Initialize() { }
            }
        }
    }
}

using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace PartyJanna
{
    public static class Config
    {
        private const string MenuName = "PartyJanna";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to PartyJanna settings menu!");

            Modes.Initialize();
        }

        public static void Initialize() { }

        public static class Modes
        {
            private static readonly Menu Menu;

            static Modes()
            {
                Menu = Config.Menu.AddSubMenu("Modes");

                Draw.Initialize();
                Menu.AddSeparator();

                Items.Initialize();
                Menu.AddSeparator();

                AutoShield.Initialize();
                Menu.AddSeparator();

                Combo.Initialize();
                Menu.AddSeparator();

                Flee.Initialize();
                Menu.AddSeparator();

                Harass.Initialize();
                Menu.AddSeparator();
            }

            public static void Initialize()
            {
            }

            public static class Draw
            {
                private static readonly CheckBox _drawQ;
                private static readonly CheckBox _drawW;
                private static readonly CheckBox _drawE;
                private static readonly CheckBox _drawR;

                public static bool DrawQ
                {
                    get { return _drawQ.CurrentValue; }
                }
                public static bool DrawW
                {
                    get { return _drawW.CurrentValue; }
                }
                public static bool DrawE
                {
                    get { return _drawE.CurrentValue; }
                }
                public static bool DrawR
                {
                    get { return _drawR.CurrentValue; }
                }

                static Draw()
                {
                    Menu.AddGroupLabel("Draw");

                    _drawQ = Menu.Add("drawQ", new CheckBox("Draw Q Range"));
                    _drawW = Menu.Add("drawW", new CheckBox("Draw W Range"));
                    _drawE = Menu.Add("drawE", new CheckBox("Draw E Range"));
                    _drawR = Menu.Add("drawR", new CheckBox("Draw R Range"));
                }

                public static void Initialize() { }
            }

            public static class Items
            {
                private static readonly CheckBox _useItems;

                public static bool UseItems
                {
                    get { return _useItems.CurrentValue; }
                }

                static Items()
                {
                    Menu.AddGroupLabel("Items");

                    _useItems = Menu.Add("useItems", new CheckBox("Use Items"));
                }

                public static void Initialize() { }
            }

            public static class AutoShield
            {
                private static readonly CheckBox _boostAD;
                private static readonly ComboBox _priorMode;
                private static readonly List<Slider> _sliders;
                private static readonly List<AIHeroClient> _heros;

                public static bool BoostAD
                {
                    get { return _boostAD.CurrentValue; }
                }
                public static int PriorMode
                {
                    get { return _priorMode.SelectedIndex; }
                }
                public static List<Slider> Sliders
                {
                    get { return _sliders; }
                }
                public static List<AIHeroClient> Heros
                {
                    get { return _heros; }
                }

                static AutoShield()
                {
                    Menu.AddGroupLabel("AutoShield");

                    _boostAD = Menu.Add("autoShieldBoostAd", new CheckBox("Use E to boost ally AD"));
                    Menu.AddSeparator(13);

                    _priorMode = Menu.Add("autoShieldPriorMode", new ComboBox("AutoShield Priority Mode:", 0, new string[] { "Lowest Health", "Priority Level" }));
                    Menu.AddSeparator(13);

                    _sliders = new List<Slider>();
                    _heros = new List<AIHeroClient>();

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        if (ally.ChampionName != Program.ChampName)
                        {
                            Slider PrioritySlider = Menu.Add<Slider>(ally.ChampionName, new Slider(string.Format("{0} ({1})", ally.ChampionName, ally.Name), 1, 1, EntityManager.Heroes.Allies.Count - 1));

                            Menu.AddSeparator(13);

                            _sliders.Add(PrioritySlider);

                            _heros.Add(ally);
                        }
                        else
                        {
                            _sliders.Add(new Slider("Janna"));
                            _heros.Add(ally);
                        }
                    }
                }

                public static void Initialize() { }
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly Slider _qUseRange;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static int QUseRange
                {
                    get { return _qUseRange.CurrentValue; }
                }

                static Combo()
                {
                    Menu.AddGroupLabel("Combo");

                    _useQ = Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Use W"));
                    Menu.AddSeparator();

                    _qUseRange = Menu.Add<Slider>("qUseRangeCombo", new Slider("Use Q at range:", 1100, 1100, 1700));
                }

                public static void Initialize() { }
            }

            public static class Flee
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                //private static readonly CheckBox _useR;
                private static readonly Slider _qUseRange;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                /*public static bool UseR
                {
                    get { return _useR.CurrentValue; }
                }*/
                public static int QUseRange
                {
                    get { return _qUseRange.CurrentValue; }
                }

                static Flee()
                {
                    Menu.AddGroupLabel("Flee");

                    _useQ = Menu.Add("fleeUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("fleeUseW", new CheckBox("Use W"));
                    //_useR = Menu.Add("comboUseR", new CheckBox("Use R", false));
                    Menu.AddSeparator();

                    _qUseRange = Menu.Add<Slider>("qUseRangeFlee", new Slider("Use Q at range:", 1100, 1100, 1700));
                }

                public static void Initialize() { }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _autoHarass;
                private static readonly Slider _autoHarassManaPercent;
                private static readonly Slider _qUseRange;

                public static bool UseQ
                {
                    get { return _useQ.CurrentValue; }
                }
                public static bool UseW
                {
                    get { return _useW.CurrentValue; }
                }
                public static bool AutoHarass
                {
                    get { return _autoHarass.CurrentValue; }
                }
                public static int AutoHarassManaPercent
                {
                    get { return _autoHarassManaPercent.CurrentValue; }
                }
                public static int QUseRange
                {
                    get { return _qUseRange.CurrentValue; }
                }

                static Harass()
                {
                    Menu.AddGroupLabel("Harass");

                    _useQ = Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    Menu.AddSeparator(13);

                    _qUseRange = Menu.Add<Slider>("qUseRangeHarass", new Slider("Use Q at range:", 1100, 1100, 1700));
                    Menu.AddSeparator();

                    _useW = Menu.Add("harassUseW", new CheckBox("Use W"));
                    Menu.AddSeparator();

                    _autoHarass = Menu.Add("autoHarass", new CheckBox("Auto Harass with W at mana %"));
                    Menu.AddSeparator(13);

                    _autoHarassManaPercent = Menu.Add<Slider>("autoHarassManaPercent", new Slider("Auto Harass min. mana %:", 75));
                }

                public static void Initialize() { }
            }
        }
    }
}

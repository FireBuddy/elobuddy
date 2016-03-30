using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;
using System;

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

            Settings.Initialize();
        }

        public static void Initialize() { }

        public static class Settings
        {
            private static readonly Menu Menu;

            static Settings()
            {
                Menu = Config.Menu.AddSubMenu("Settings");

                Draw.Initialize();
                Menu.AddSeparator(13);

                AntiGapcloser.Initialize();
                Menu.AddSeparator(13);

                Interrupter.Initialize();
                Menu.AddSeparator(13);

                Items.Initialize();
                Menu.AddSeparator(13);

                AutoShield.Initialize();
                Menu.AddSeparator(13);

                Combo.Initialize();
                Menu.AddSeparator(13);

                Flee.Initialize();
                Menu.AddSeparator(13);

                Harass.Initialize();
                Menu.AddSeparator(13);

                Humanizer.Initialize();
                Menu.AddSeparator(13);

                SkinHack.Initialize();
                Menu.AddSeparator(13);
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
                    Menu.AddGroupLabel("Draw Settings");

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

            public static class AntiGapcloser
            {
                private static readonly CheckBox _antiGapcloser;

                public static bool AntiGap
                {
                    get { return _antiGapcloser.CurrentValue; }
                }

                static AntiGapcloser()
                {
                    Menu.AddGroupLabel("Anti-Gapcloser Settings");

                    _antiGapcloser = Menu.Add("antiGapcloser", new CheckBox("Anti-Gapcloser"));
                }

                public static void Initialize() { }
            }

            public static class Interrupter
            {
                private static readonly CheckBox _qInterrupt;
                private static readonly CheckBox _qInterruptDangerous;
                private static readonly CheckBox _rInterruptDangerous;

                public static bool QInterrupt
                {
                    get { return _qInterrupt.CurrentValue; }
                }
                public static bool QInterruptDangerous
                {
                    get { return _qInterruptDangerous.CurrentValue; }
                }
                public static bool RInterruptDangerous
                {
                    get { return _rInterruptDangerous.CurrentValue; }
                }

                static Interrupter()
                {
                    Menu.AddGroupLabel("Interrupter Settings");

                    _qInterrupt = Menu.Add("qInterrupt", new CheckBox("Interrupt low/med-danger spells with Q"));
                    Menu.AddSeparator(13);

                    _qInterruptDangerous = Menu.Add("rInterrupt", new CheckBox("Interrupt high-danger spells with Q"));
                    Menu.AddSeparator(13);

                    _rInterruptDangerous = Menu.Add("rInterruptDangerous", new CheckBox("Interrupt high-danger spells with R"));
                }

                public static void Initialize() { }
            }

            public static class AutoShield
            {
                private static readonly CheckBox _boostAD;
                private static readonly CheckBox _selfShield;
                private static readonly CheckBox _turretShieldMinion, _turretShieldChampion;
                private static readonly ComboBox _priorMode;
                private static readonly List<Slider> _sliders;
                private static readonly List<AIHeroClient> _heros;
                private static readonly List<CheckBox> _shieldAllyList;

                public static bool BoostAD
                {
                    get { return _boostAD.CurrentValue; }
                }
                public static bool SelfShield
                {
                    get { return _selfShield.CurrentValue; }
                }
                public static bool TurretShieldMinion
                {
                    get { return _turretShieldMinion.CurrentValue; }
                }
                public static bool TurretShieldChampion
                {
                    get { return _turretShieldChampion.CurrentValue; }
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
                public static List<CheckBox> ShieldAllyList
                {
                    get { return _shieldAllyList; }
                }

                static AutoShield()
                {
                    _shieldAllyList = new List<CheckBox>();

                    Menu.AddGroupLabel("AutoShield Settings");

                    foreach (var ally2 in EntityManager.Heroes.Allies)
                    {
                        _shieldAllyList.Add(Menu.Add<CheckBox>("Shield " + ally2.ChampionName, new CheckBox(string.Format("Shield {0} ({1})", ally2.ChampionName, ally2.Name))));
                    }

                    Menu.AddSeparator(13);

                    _boostAD = Menu.Add("autoShieldBoostAd", new CheckBox("Boost ally AutoAttack with Shield"));
                    Menu.AddSeparator(13);

                    _selfShield = Menu.Add("selfShield", new CheckBox("Shield Yourself from Basic Attacks"));
                    Menu.AddSeparator(13);

                    _turretShieldMinion = Menu.Add("turretShieldMinion", new CheckBox("Shield Turrets from Enemy Minions", false));
                    Menu.AddSeparator(13);

                    _turretShieldChampion = Menu.Add("turretShieldChampion", new CheckBox("Shield Turrets from Enemy Champions"));
                    Menu.AddSeparator(13);

                    _priorMode = Menu.Add("autoShieldPriorMode", new ComboBox("AutoShield Priority Mode:", 0, new string[] { "Lowest Health", "Priority Level" }));
                    Menu.AddSeparator(13);

                    _sliders = new List<Slider>();
                    _heros = new List<AIHeroClient>();

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        Slider PrioritySlider = Menu.Add<Slider>(ally.ChampionName, new Slider(string.Format("{0} Priority:", ally.ChampionName, ally.Name), 1, 1, EntityManager.Heroes.Allies.Count - 1));

                        Menu.AddSeparator(13);

                        _sliders.Add(PrioritySlider);

                        _heros.Add(ally);
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
                    Menu.AddGroupLabel("Combo Settings");

                    _useQ = Menu.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("comboUseW", new CheckBox("Use W"));
                    Menu.AddSeparator();

                    _qUseRange = Menu.Add<Slider>("qUseRangeCombo", new Slider("Use Q at range:", 1000, 1000, 1100));
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
                    Menu.AddGroupLabel("Flee Settings");

                    _useQ = Menu.Add("fleeUseQ", new CheckBox("Use Q"));
                    _useW = Menu.Add("fleeUseW", new CheckBox("Use W"));
                    //_useR = Menu.Add("comboUseR", new CheckBox("Use R", false));
                    Menu.AddSeparator();

                    _qUseRange = Menu.Add<Slider>("qUseRangeFlee", new Slider("Use Q at range:", 1000, 1000, 1100));
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
                    Menu.AddGroupLabel("Harass Settings");

                    _useQ = Menu.Add("harassUseQ", new CheckBox("Use Q"));
                    Menu.AddSeparator(13);

                    _qUseRange = Menu.Add<Slider>("qUseRangeHarass", new Slider("Use Q at range:", 1000, 1000, 1100));
                    Menu.AddSeparator();

                    _useW = Menu.Add("harassUseW", new CheckBox("Use W"));
                    Menu.AddSeparator();

                    _autoHarass = Menu.Add("autoHarass", new CheckBox("Auto Harass with W at mana %"));
                    Menu.AddSeparator(13);

                    _autoHarassManaPercent = Menu.Add<Slider>("autoHarassManaPercent", new Slider("Auto Harass min. mana %:", 75));
                }

                public static void Initialize() { }
            }

            public static class Humanizer
            {
                private static readonly CheckBox _qCastDelayEnabled;
                private static readonly CheckBox _eCastDelayEnabled;
                private static readonly CheckBox _rCastDelayEnabled;
                private static readonly Slider _qCastDelay;
                private static readonly Slider _eCastDelay;
                private static readonly Slider _rCastDelay;
                private static readonly CheckBox _qRndmDelay;
                private static readonly CheckBox _eRndmDelay;
                private static readonly CheckBox _rRndmDelay;

                public static bool QCastDelayEnabled
                {
                    get { return _qCastDelayEnabled.CurrentValue; }
                }
                public static bool ECastDelayEnabled
                {
                    get { return _eCastDelayEnabled.CurrentValue; }
                }
                public static bool RCastDelayEnabled
                {
                    get { return _rCastDelayEnabled.CurrentValue; }
                }
                public static int QCastDelay
                {
                    get { return _qCastDelay.CurrentValue; }
                }
                public static int ECastDelay
                {
                    get { return _eCastDelay.CurrentValue; }
                }
                public static int RCastDelay
                {
                    get { return _rCastDelay.CurrentValue; }
                }
                public static bool QRndmDelay
                {
                    get { return _qRndmDelay.CurrentValue; }
                }
                public static bool ERndmDelay
                {
                    get { return _eRndmDelay.CurrentValue; }
                }
                public static bool RRndmDelay
                {
                    get { return _rRndmDelay.CurrentValue; }
                }

                static Humanizer()
                {
                    Menu.AddGroupLabel("Humanizer Settings");

                    _qCastDelayEnabled = Menu.Add("qCastDelayEnabled", new CheckBox("Enabled", false));
                    _qCastDelay = Menu.Add<Slider>("qCastDelay", new Slider("Q Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu.AddSeparator();

                    _eCastDelayEnabled = Menu.Add("eCastDelayEnabled", new CheckBox("Enabled", false));
                    _eCastDelay = Menu.Add<Slider>("eCastDelay", new Slider("E Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu.AddSeparator();

                    _rCastDelayEnabled = Menu.Add("rCastDelayEnabled", new CheckBox("Enabled", false));
                    _rCastDelay = Menu.Add<Slider>("rCastDelay", new Slider("R Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu.AddSeparator();

                    _qRndmDelay = Menu.Add("qRndmDelay", new CheckBox("Randomize Q Cast Delay"));
                    Menu.AddSeparator();

                    _eRndmDelay = Menu.Add("eRndmDelay", new CheckBox("Randomize E Cast Delay"));
                    Menu.AddSeparator();

                    _rRndmDelay = Menu.Add("rRndmDelay", new CheckBox("Randomize R Cast Delay"));
                    Menu.AddSeparator();
                }

                public static void Initialize() { }
            }

            public static class SkinHack
            {
                private static readonly CheckBox _skinHackEnabled;
                private static readonly Slider _skinId;

                public static bool SkinHackEnabled
                {
                    get { return _skinHackEnabled.CurrentValue; }
                }
                public static int SkinID
                {
                    get { return _skinId.CurrentValue; }
                }

                static SkinHack()
                {
                    Menu.AddGroupLabel("Skin Hack Settings");

                    _skinHackEnabled = Menu.Add("skinHackEnabled", new CheckBox("Enabled", false));
                    _skinId = Menu.Add<Slider>("skinId", new Slider("Skin ID:", 0, 0, 11));

                    _skinId.OnValueChange += OnSkinIdChange;
                    _skinHackEnabled.OnValueChange += OnSkinHackToggle;

                    Player.Instance.SetSkinId(SkinID);
                }

                private static void OnSkinHackToggle(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    if (!args.NewValue)
                        Player.Instance.SetSkinId(0);
                    else
                        Player.Instance.SetSkinId(SkinID);
                }

                private static void OnSkinIdChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    if (SkinHackEnabled)
                    Player.Instance.SetSkinId(args.NewValue);
                }

                public static void Initialize() { }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace PartyMorg
{
    public static class Config
    {
        private const string MenuName = "PartyMorg";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("Welcome to PartyMorg settings menu!");

            Settings.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Settings
        {
            private static readonly Menu Menu0,
                Menu1,
                Menu2,
                Menu3,
                Menu4,
                Menu5,
                Menu6,
                Menu7,
                Menu8,
                Menu9,
                Menu10,
                Menu11;

            static Settings()
            {
                Menu0 = Menu.AddSubMenu("Draw");
                Draw.Initialize();

                Menu1 = Menu.AddSubMenu("Anti-Gapcloser");
                AntiGapcloser.Initialize();

                Menu2 = Menu.AddSubMenu("Interrupter");
                Interrupter.Initialize();

                Menu3 = Menu.AddSubMenu("Items");
                Items.Initialize();

                Menu4 = Menu.AddSubMenu("Auto-Shield");
                AutoShield.Initialize();

                Menu5 = Menu.AddSubMenu("Combo");
                Combo.Initialize();

                Menu6 = Menu.AddSubMenu("Flee");
                Flee.Initialize();

                Menu7 = Menu.AddSubMenu("Harass");
                Harass.Initialize();

                Menu10 = Menu.AddSubMenu("Lane Clear");
                LaneClear.Initialize();

                Menu11 = Menu.AddSubMenu("Jungle Clear");
                JungleClear.Initialize();

                Menu8 = Menu.AddSubMenu("Humanizer");
                Humanizer.Initialize();

                Menu9 = Menu.AddSubMenu("Skin Hack");
                SkinHack.Initialize();
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

                static Draw()
                {
                    Menu0.AddGroupLabel("Draw Settings");

                    _drawQ = Menu0.Add("drawQ", new CheckBox("Draw Q Range"));
                    _drawW = Menu0.Add("drawW", new CheckBox("Draw W Range"));
                    _drawE = Menu0.Add("drawE", new CheckBox("Draw E Range"));
                    _drawR = Menu0.Add("drawR", new CheckBox("Draw R Range"));
                }

                public static bool DrawQ => _drawQ.CurrentValue;

                public static bool DrawW => _drawW.CurrentValue;

                public static bool DrawE => _drawE.CurrentValue;

                public static bool DrawR => _drawR.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class Items
            {
                private static readonly CheckBox _useItems, _useItemsComboOnly;
                private static readonly Slider _allyHpPercentageDamage, _allyHpPercentageCc;

                static Items()
                {
                    ISAllyList = new List<CheckBox>();
                    FOTMAllyList = new List<CheckBox>();
                    MCAllyList = new List<CheckBox>();
                    FQCAllyList = new List<CheckBox>();
                    TOAAllyList = new List<CheckBox>();

                    Menu3.AddGroupLabel("Items");

                    _useItems = Menu3.Add("useItems", new CheckBox("Use Items"));

                    Menu3.AddSeparator(13);

                    _useItemsComboOnly = Menu3.Add("useItemsComboOnly",
                        new CheckBox("Use Items only in Combo Mode", false));

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Locket of the Iron Solari");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        ISAllyList.Add(Menu3.Add("ironSolari" + ally.ChampionName,
                            new CheckBox($"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Face of the Mountain");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        FOTMAllyList.Add(Menu3.Add("mountain" + ally.ChampionName,
                            new CheckBox($"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Mikael's Crucible");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        MCAllyList.Add(Menu3.Add("mikael" + ally.ChampionName,
                            new CheckBox($"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Frost Queen's Claim");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        FQCAllyList.Add(Menu3.Add("frostQueen" + ally.ChampionName,
                            new CheckBox($"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Talisman of Ascension");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        TOAAllyList.Add(Menu3.Add("talisman" + ally.ChampionName,
                            new CheckBox($"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    _allyHpPercentageDamage = Menu3.Add("allyHpPercentage",
                        new Slider("Min. Ally Health on Damage (%):", 50, 1));

                    Menu3.AddSeparator(13);

                    _allyHpPercentageCc = Menu3.Add("allyHpPercentageCc",
                        new Slider("Min. Ally Health on CC (%):", 100, 1));
                }

                public static bool UseItems => _useItems.CurrentValue;

                public static bool UseItemsComboOnly => _useItemsComboOnly.CurrentValue;

                public static int AllyHpPercentageDamage => _allyHpPercentageDamage.CurrentValue;

                public static int AllyHpPercentageCC => _allyHpPercentageCc.CurrentValue;

                public static List<CheckBox> ISAllyList { get; }

                public static List<CheckBox> FOTMAllyList { get; }

                public static List<CheckBox> MCAllyList { get; }

                public static List<CheckBox> FQCAllyList { get; }

                public static List<CheckBox> TOAAllyList { get; }

                public static void Initialize()
                {
                }
            }

            public static class AntiGapcloser
            {
                private static readonly CheckBox _antiGapcloser;

                static AntiGapcloser()
                {
                    Menu1.AddGroupLabel("Anti-Gapcloser Settings");

                    _antiGapcloser = Menu1.Add("antiGapcloser", new CheckBox("Use Q on Gapclosers"));
                }

                public static bool AntiGap => _antiGapcloser.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class Interrupter
            {
                private static readonly CheckBox _qInterrupt;
                private static readonly CheckBox _qInterruptDangerous;

                static Interrupter()
                {
                    Menu2.AddGroupLabel("Interrupter Settings");

                    _qInterrupt = Menu2.Add("qInterrupt", new CheckBox("Interrupt low/med-danger spells with Q"));
                    Menu2.AddSeparator(13);

                    _qInterruptDangerous = Menu2.Add("rInterrupt", new CheckBox("Interrupt high-danger spells with Q"));
                    Menu2.AddSeparator(13);
                }

                public static bool QInterrupt => _qInterrupt.CurrentValue;

                public static bool QInterruptDangerous => _qInterruptDangerous.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class AutoShield
            {
                private static readonly ComboBox _priorMode;

                static AutoShield()
                {
                    ShieldAllyList = new List<CheckBox>();
                    ShieldSpellList = new List<CheckBox>();

                    Menu4.AddGroupLabel("Auto-Shield Settings");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        ShieldAllyList.Add(Menu4.Add("shield" + ally.ChampionName,
                            new CheckBox($"Shield {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu4.AddSeparator(13);

                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        for (var i = 0; i <= 185; i++)
                        {
                            if (MissileDatabase.missileDatabase[i, 2] == enemy.ChampionName)
                                ShieldSpellList.Add(Menu4.Add(MissileDatabase.missileDatabase[i, 0] + i,
                                    new CheckBox(
                                        $"Shield from {MissileDatabase.missileDatabase[i, 2]}'s {MissileDatabase.missileDatabase[i, 1]} ({MissileDatabase.missileDatabase[i, 0]})                                                 {i}")));
                        }
                    }

                    Menu4.AddSeparator(13);

                    _priorMode = Menu4.Add("autoShieldPriorMode",
                        new ComboBox("AutoShield Priority Mode:", 0, "Lowest Health", "Priority Level"));
                    Menu4.AddSeparator(13);

                    Sliders = new List<Slider>();
                    Heros = new List<AIHeroClient>();

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        var PrioritySlider = Menu4.Add(ally.ChampionName,
                            new Slider(string.Format("{0} Priority:", ally.ChampionName, ally.Name), 1, 1,
                                EntityManager.Heroes.Allies.Count));

                        Menu4.AddSeparator(13);

                        Sliders.Add(PrioritySlider);

                        Heros.Add(ally);
                    }
                }

                public static int PriorMode => _priorMode.SelectedIndex;

                public static List<Slider> Sliders { get; }

                public static List<AIHeroClient> Heros { get; }

                public static List<CheckBox> ShieldAllyList { get; }

                public static List<CheckBox> ShieldSpellList { get; }

                public static void Initialize()
                {
                }
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useR;
                private static readonly CheckBox _useQBeforeW;
                private static readonly CheckBox _flashUlt;
                private static readonly CheckBox _ultZhonya;
                private static readonly CheckBox _wImmobileOnly;
                private static readonly Slider _qMinHitChance;
                private static readonly Slider _rMinEnemies;
                private static readonly Slider _ultMinRange;

                static Combo()
                {
                    Menu5.AddGroupLabel("Combo Settings");

                    _useQ = Menu5.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu5.Add("comboUseW", new CheckBox("Use W"));
                    _useR = Menu5.Add("comboUseR", new CheckBox("Use R"));
                    Menu5.AddSeparator();

                    _qMinHitChance = Menu5.Add("comboQMinHitChance",
                        new Slider("Q Min. Hit Chance (%):", 75, 50));
                    Menu5.AddSeparator();

                    _useQBeforeW = Menu5.Add("comboUseQBeforeW", new CheckBox("Use W after Q only"));
                    _flashUlt = Menu5.Add("flashUlt", new CheckBox("Use Flash + Ultimate", false));
                    _ultZhonya = Menu5.Add("ultZhonya", new CheckBox("Use Zhonya with Ultimate"));
                    _wImmobileOnly = Menu5.Add("comboWImmobileOnly", new CheckBox("W Only Immobile Enemies"));
                    Menu5.AddSeparator();

                    _rMinEnemies = Menu5.Add("rMinEnemies",
                        new Slider("Min. Enemies Around to use Ultimate:", 1, 1, EntityManager.Heroes.Enemies.Count));
                    _ultMinRange = Menu5.Add("ultMinRange",
                        new Slider("Min. Range from Enemy to use Ultimate:", 470, 1,
                            Convert.ToInt32(SpellManager.R.Range)));
                }

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static bool UseR => _useR.CurrentValue;

                public static bool UseQBeforeW => _useQBeforeW.CurrentValue;

                public static bool FlashUlt => _flashUlt.CurrentValue;

                public static bool UltZhonya => _ultZhonya.CurrentValue;

                public static bool WImmobileOnly => _wImmobileOnly.CurrentValue;

                public static int QMinHitChance => _qMinHitChance.CurrentValue;

                public static int RMinEnemies => _rMinEnemies.CurrentValue;

                public static int UltMinRange => _ultMinRange.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class Flee
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _useQBeforeW;
                private static readonly Slider _qMinHitChance;
                private static readonly CheckBox _wImmobileOnly;

                static Flee()
                {
                    Menu6.AddGroupLabel("Flee Settings");

                    _useQ = Menu6.Add("fleeUseQ", new CheckBox("Use Q"));
                    _useW = Menu6.Add("fleeUseW", new CheckBox("Use W"));
                    Menu6.AddSeparator();

                    _useQBeforeW = Menu6.Add("fleeUseQBeforeW", new CheckBox("Use W after Q only"));
                    _wImmobileOnly = Menu6.Add("fleeWImmobileOnly", new CheckBox("W Only Immobile Enemies"));
                    Menu6.AddSeparator();

                    _qMinHitChance = Menu6.Add("fleeQMinHitChance", new Slider("Q Min. Hit Chance (%):", 75, 50));
                }

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static bool UseQBeforeW => _useQBeforeW.CurrentValue;

                public static int QMinHitChance => _qMinHitChance.CurrentValue;

                public static bool WImmobileOnly => _wImmobileOnly.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _wImmobileOnly;
                private static readonly CheckBox _useQBeforeW;
                private static readonly Slider _qMinHitChance;

                static Harass()
                {
                    Menu7.AddGroupLabel("Harass Settings");

                    _useQ = Menu7.Add("harassUseQ", new CheckBox("Use Q"));

                    _useW = Menu7.Add("harassUseW", new CheckBox("Use W"));
                    Menu7.AddSeparator();

                    _useQBeforeW = Menu7.Add("harassUseQBeforeW", new CheckBox("Use W after Q only"));
                    _wImmobileOnly = Menu7.Add("harassWImmobileOnly", new CheckBox("W Only Immobile Enemies"));
                    Menu7.AddSeparator();

                    _qMinHitChance = Menu7.Add("harassQMinHitChance",
                        new Slider("Q Min. Hit Chance (%):", 75, 50));
                }

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static bool UseQBeforeW => _useQBeforeW.CurrentValue;

                public static int QMinHitChance => _qMinHitChance.CurrentValue;

                public static bool WImmobileOnly => _wImmobileOnly.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class LaneClear
            {
                private static readonly CheckBox _useW;
                private static readonly Slider _minionsToUseW;

                static LaneClear()
                {
                    Menu10.AddGroupLabel("Lane Clear Settings");

                    _useW = Menu10.Add("laneClearUseW", new CheckBox("Use W"));
                    Menu10.AddSeparator();

                    _minionsToUseW = Menu10.Add("minionsToUseW", new Slider("Minions to use W:", 3, 1, 7));
                }

                public static bool UseW => _useW.CurrentValue;

                public static int MinionsToUseW => _minionsToUseW.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class JungleClear
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                static JungleClear()
                {
                    Menu11.AddGroupLabel("Jungle Clear Settings");

                    _useQ = Menu11.Add("jungleClearUseQ", new CheckBox("Use Q"));
                    Menu11.AddSeparator();

                    _useW = Menu11.Add("jungleClearUseW", new CheckBox("Use W"));
                    Menu11.AddSeparator();
                }

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static void Initialize()
                {
                }
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

                static Humanizer()
                {
                    Menu8.AddGroupLabel("Humanizer Settings");

                    _qCastDelayEnabled = Menu8.Add("qCastDelayEnabled", new CheckBox("Enabled", false));
                    _qCastDelay = Menu8.Add("qCastDelay",
                        new Slider("Q Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu8.AddSeparator();

                    _eCastDelayEnabled = Menu8.Add("eCastDelayEnabled", new CheckBox("Enabled", false));
                    _eCastDelay = Menu8.Add("eCastDelay",
                        new Slider("E Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu8.AddSeparator();

                    _rCastDelayEnabled = Menu8.Add("rCastDelayEnabled", new CheckBox("Enabled", false));
                    _rCastDelay = Menu8.Add("rCastDelay",
                        new Slider("R Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu8.AddSeparator();

                    _qRndmDelay = Menu8.Add("qRndmDelay", new CheckBox("Randomize Q Cast Delay"));
                    Menu8.AddSeparator();

                    _eRndmDelay = Menu8.Add("eRndmDelay", new CheckBox("Randomize E Cast Delay"));
                    Menu8.AddSeparator();

                    _rRndmDelay = Menu8.Add("rRndmDelay", new CheckBox("Randomize R Cast Delay"));
                    Menu8.AddSeparator();
                }

                public static bool QCastDelayEnabled => _qCastDelayEnabled.CurrentValue;

                public static bool ECastDelayEnabled => _eCastDelayEnabled.CurrentValue;

                public static bool RCastDelayEnabled => _rCastDelayEnabled.CurrentValue;

                public static int QCastDelay => _qCastDelay.CurrentValue;

                public static int ECastDelay => _eCastDelay.CurrentValue;

                public static int RCastDelay => _rCastDelay.CurrentValue;

                public static bool QRndmDelay => _qRndmDelay.CurrentValue;

                public static bool ERndmDelay => _eRndmDelay.CurrentValue;

                public static bool RRndmDelay => _rRndmDelay.CurrentValue;

                public static void Initialize()
                {
                }
            }

            private static class SkinHack
            {
                private static readonly CheckBox _skinHackEnabled;
                private static readonly Slider _skinId;

                static SkinHack()
                {
                    Menu9.AddGroupLabel("Skin Hack Settings");

                    _skinHackEnabled = Menu9.Add("skinHackEnabled", new CheckBox("Enabled", false));
                    _skinId = Menu9.Add("skinId", new Slider("Skin ID:", 0, 0, 11));

                    _skinId.OnValueChange += OnSkinIdChange;
                    _skinHackEnabled.OnValueChange += OnSkinHackToggle;

                    Player.Instance.SetSkinId(SkinID);
                }

                private static bool SkinHackEnabled => _skinHackEnabled.CurrentValue;
                private static int SkinID => _skinId.CurrentValue;

                private static void OnSkinHackToggle(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                    => Player.Instance.SetSkinId(args.NewValue == false ? 0 : SkinID);

                private static void OnSkinIdChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    if (SkinHackEnabled)
                        Player.Instance.SetSkinId(args.NewValue);
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}
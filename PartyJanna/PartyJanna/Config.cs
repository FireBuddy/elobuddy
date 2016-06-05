using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

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

        public static void Initialize()
        {
        }

        public static class Settings
        {
            private static readonly Menu Menu0, Menu1, Menu2, Menu3, Menu4, Menu5, Menu6, Menu7, Menu8, Menu9;

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

                private static readonly List<CheckBox> _isAllyList,
                    _fotmAllyList,
                    _mcAllyList,
                    _fqcAllyList,
                    _toaAllyList;

                static Items()
                {
                    _isAllyList = new List<CheckBox>();
                    _fotmAllyList = new List<CheckBox>();
                    _mcAllyList = new List<CheckBox>();
                    _fqcAllyList = new List<CheckBox>();
                    _toaAllyList = new List<CheckBox>();

                    Menu3.AddGroupLabel("Items");

                    _useItems = Menu3.Add("useItems", new CheckBox("Use Items"));

                    Menu3.AddSeparator(13);

                    _useItemsComboOnly = Menu3.Add("useItemsComboOnly",
                        new CheckBox("Use Items only in Combo Mode", false));

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Locket of the Iron Solari");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _isAllyList.Add(Menu3.Add("ironSolari" + ally.ChampionName, new CheckBox(
                            $"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Face of the Mountain");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _fotmAllyList.Add(Menu3.Add("mountain" + ally.ChampionName, new CheckBox(
                            $"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Mikael's Crucible");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _mcAllyList.Add(Menu3.Add("mikael" + ally.ChampionName, new CheckBox(
                            $"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Frost Queen's Claim");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _fqcAllyList.Add(Menu3.Add("frostQueen" + ally.ChampionName, new CheckBox(
                            $"Use on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu3.AddSeparator(13);

                    Menu3.AddGroupLabel("Talisman of Ascension");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _toaAllyList.Add(Menu3.Add("talisman" + ally.ChampionName, new CheckBox(
                            $"Use on {ally.ChampionName} ({ally.Name})")));
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

                public static IEnumerable<CheckBox> ISAllyList => _isAllyList;

                public static IEnumerable<CheckBox> FOTMAllyList => _fotmAllyList;

                public static IEnumerable<CheckBox> MCAllyList => _mcAllyList;

                public static IEnumerable<CheckBox> FQCAllyList => _fqcAllyList;

                public static IEnumerable<CheckBox> TOAAllyList => _toaAllyList;

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

                    _antiGapcloser = Menu1.Add("antiGapcloser", new CheckBox("Anti-Gapcloser"));
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
                private static readonly CheckBox _rInterruptDangerous;

                static Interrupter()
                {
                    Menu2.AddGroupLabel("Interrupter Settings");

                    _qInterrupt = Menu2.Add("qInterrupt", new CheckBox("Interrupt low/med-danger spells with Q"));
                    Menu2.AddSeparator(13);

                    _qInterruptDangerous = Menu2.Add("rInterrupt", new CheckBox("Interrupt high-danger spells with Q"));
                    Menu2.AddSeparator(13);

                    _rInterruptDangerous = Menu2.Add("rInterruptDangerous",
                        new CheckBox("Interrupt high-danger spells with R"));
                }

                public static bool QInterrupt => _qInterrupt.CurrentValue;

                public static bool QInterruptDangerous => _qInterruptDangerous.CurrentValue;

                public static bool RInterruptDangerous => _rInterruptDangerous.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class AutoShield
            {
                private static readonly CheckBox _boostAD, _boostEzQ;
                private static readonly CheckBox _selfShield;
                private static readonly CheckBox _turretShieldMinion, _turretShieldChampion;
                private static readonly CheckBox _autoUltimate;
                private static readonly ComboBox _priorMode;
                private static readonly List<Slider> _sliders, _ultSliders;
                private static readonly List<AIHeroClient> _heros;
                private static readonly List<CheckBox> _shieldAllyList, _shieldSpellList, _ultAllyList;

                static AutoShield()
                {
                    _shieldAllyList = new List<CheckBox>();
                    _shieldSpellList = new List<CheckBox>();
                    _ultAllyList = new List<CheckBox>();

                    Menu4.AddGroupLabel("Auto-Shield Settings");

                    _boostAD = Menu4.Add("autoShieldBoostAd", new CheckBox("Boost ADCarry Basic Attacks with Shield"));
                    Menu4.AddSeparator(13);

                    foreach (var ally in EntityManager.Heroes.Allies.Where(x => x.ChampionName == "Ezreal"))
                    {
                        _boostEzQ = Menu4.Add("autoShieldBoostEzQ", new CheckBox("Boost Ezreal Q with Shield"));
                        Menu4.AddSeparator(13);
                    }

                    _selfShield = Menu4.Add("selfShield", new CheckBox("Shield Yourself from Basic Attacks"));
                    Menu4.AddSeparator(13);

                    _turretShieldMinion = Menu4.Add("turretShieldMinion",
                        new CheckBox("Shield Turrets from Enemy Minions", false));
                    Menu4.AddSeparator(13);

                    _turretShieldChampion = Menu4.Add("turretShieldChampion",
                        new CheckBox("Shield Turrets from Enemy Champions"));
                    Menu4.AddSeparator(13);

                    _priorMode = Menu4.Add("autoShieldPriorMode",
                        new ComboBox("AutoShield Priority Mode:", 0, "Lowest Health", "Priority Level"));
                    Menu4.AddSeparator(13);

                    Menu4.AddGroupLabel("Janna Shield");

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _shieldAllyList.Add(Menu4.Add("shield" + ally.ChampionName, new CheckBox(
                            $"Shield {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu4.AddSeparator(13);

                    _sliders = new List<Slider>();
                    _heros = new List<AIHeroClient>();

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _sliders.Add(Menu4.Add("prior" + ally.ChampionName, new Slider(
                            $"{ally.ChampionName}'s Priority:", 1, 1, EntityManager.Heroes.Allies.Count)));

                        Menu4.AddSeparator(13);

                        _heros.Add(ally);
                    }

                    foreach (var enemy in EntityManager.Heroes.Enemies)
                    {
                        for (var i = 0; i <= 185; i++)
                        {
                            if (MissileDatabase.missileDatabase[i, 2] == enemy.ChampionName)
                                _shieldSpellList.Add(Menu4.Add(MissileDatabase.missileDatabase[i, 0] + i, new CheckBox(
                                    $"Shield from {MissileDatabase.missileDatabase[i, 2]}'s {MissileDatabase.missileDatabase[i, 1]} ({MissileDatabase.missileDatabase[i, 0]})                                                 {i}")));
                        }
                    }

                    Menu4.AddGroupLabel("Janna Ultimate");

                    _autoUltimate = Menu4.Add("autoUltimate", new CheckBox("Auto-Ult Enabled", false));

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _ultAllyList.Add(Menu4.Add("autoUlt" + ally.ChampionName, new CheckBox(
                            $"Ultimate on {ally.ChampionName} ({ally.Name})")));
                    }

                    Menu4.AddSeparator(13);

                    _ultSliders = new List<Slider>();

                    foreach (var ally in EntityManager.Heroes.Allies)
                    {
                        _ultSliders.Add(Menu4.Add("ultHealth" + ally.ChampionName, new Slider(
                            $"{ally.ChampionName}'s Health (%):", 50, 1)));

                        Menu4.AddSeparator(13);
                    }
                }


                public static bool BoostAD => _boostAD.CurrentValue;

                public static bool BoostEzQ => _boostEzQ.CurrentValue;

                public static bool SelfShield => _selfShield.CurrentValue;

                public static bool TurretShieldMinion => _turretShieldMinion.CurrentValue;

                public static bool TurretShieldChampion => _turretShieldChampion.CurrentValue;

                public static int PriorMode => _priorMode.SelectedIndex;

                public static IEnumerable<Slider> Sliders => _sliders;

                public static IEnumerable<Slider> UltSliders => _ultSliders;

                public static IEnumerable<AIHeroClient> Heros => _heros;

                public static IEnumerable<CheckBox> ShieldAllyList => _shieldAllyList;

                public static IEnumerable<CheckBox> ShieldSpellList => _shieldSpellList;

                public static IEnumerable<CheckBox> UltAllyList => _ultAllyList;

                public static bool AutoUltimate => _autoUltimate.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class Combo
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                static Combo()
                {
                    Menu5.AddGroupLabel("Combo Settings");

                    _useQ = Menu5.Add("comboUseQ", new CheckBox("Use Q"));
                    _useW = Menu5.Add("comboUseW", new CheckBox("Use W"));
                }

                //private static readonly Slider _qUseRange;

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class Flee
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;

                static Flee()
                {
                    Menu6.AddGroupLabel("Flee Settings");

                    _useQ = Menu6.Add("fleeUseQ", new CheckBox("Use Q"));
                    _useW = Menu6.Add("fleeUseW", new CheckBox("Use W"));
                }

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static void Initialize()
                {
                }
            }

            public static class Harass
            {
                private static readonly CheckBox _useQ;
                private static readonly CheckBox _useW;
                private static readonly CheckBox _autoHarass;
                private static readonly Slider _autoHarassManaPercent;

                static Harass()
                {
                    Menu7.AddGroupLabel("Harass Settings");

                    _useQ = Menu7.Add("harassUseQ", new CheckBox("Use Q"));
                    Menu7.AddSeparator(13);

                    _useW = Menu7.Add("harassUseW", new CheckBox("Use W"));
                    Menu7.AddSeparator();

                    _autoHarass = Menu7.Add("autoHarass", new CheckBox("Auto Harass with W at mana %"));
                    Menu7.AddSeparator(13);

                    _autoHarassManaPercent = Menu7.Add("autoHarassManaPercent",
                        new Slider("Auto Harass min. mana %:", 75, 1));
                }

                public static bool UseQ => _useQ.CurrentValue;

                public static bool UseW => _useW.CurrentValue;

                public static bool AutoHarass => _autoHarass.CurrentValue;

                public static int AutoHarassManaPercent => _autoHarassManaPercent.CurrentValue;

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
                    _qCastDelay = Menu8.Add("qCastDelay", new Slider("Q Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu8.AddSeparator();

                    _eCastDelayEnabled = Menu8.Add("eCastDelayEnabled", new CheckBox("Enabled", false));
                    _eCastDelay = Menu8.Add("eCastDelay", new Slider("E Cast Delay (1sec = 1000ms):", 500, 250, 1000));
                    Menu8.AddSeparator();

                    _rCastDelayEnabled = Menu8.Add("rCastDelayEnabled", new CheckBox("Enabled", false));
                    _rCastDelay = Menu8.Add("rCastDelay", new Slider("R Cast Delay (1sec = 1000ms):", 500, 250, 1000));
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

                    Menu4.AddSeparator(13);

                    _skinId = Menu9.Add("skinId", new Slider("Skin ID:", 0, 0, 11));

                    _skinId.OnValueChange += OnSkinIdChange;
                    _skinHackEnabled.OnValueChange += OnSkinHackToggle;

                    Player.Instance.SetSkinId(SkinID);
                }

                private static bool SkinHackEnabled => _skinHackEnabled.CurrentValue;

                private static int SkinID => _skinId.CurrentValue;

                private static void OnSkinHackToggle(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    Player.Instance.SetSkinId(args.NewValue == false ? 0 : SkinID);
                }

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
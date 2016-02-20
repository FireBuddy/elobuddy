using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace PartyJanna
{
    public static class Config
    {
        public static void Init()
        { }

        public const string AddonChampion = "Janna";
        public const string AddonName = "PartyJanna";

        private const bool ComboFunction = true;
        private const bool FleeFunction = true;
        private const bool HarassFunction = true;
        private const bool JungleClearFunction = true;
        private const bool LaneClearFunction = true;
        private const bool KillStealerFunction = true;

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(AddonName, AddonName.ToLower());
            Menu.AddGroupLabel(string.Format("Welcome to {0}'s settings menu!\nYour feedback would be much appreciated!", AddonName));

            Spells.Init();
            Combo.Init();
            Flee.Init();
            Harass.Init();
            JungleClear.Init();
            LaneClear.Init();
            KillStealer.Init();
            Draw.Init();
        }

        public static class Spells
        {
            public static void Init()
            { }

            public static Spell.Skillshot Q { get; private set; }
            public static Spell.Targeted W { get; private set; }
            public static Spell.Targeted E { get; private set; }
            public static Spell.Active R { get; private set; }

            static Spells()
            {
                Q = new Spell.Chargeable(SpellSlot.Q, 1100, 1700, 3);
                W = new Spell.Targeted(SpellSlot.W, 600);
                E = new Spell.Targeted(SpellSlot.E, 800);
                R = new Spell.Active(SpellSlot.R, 725);
            }
        }

        public static class RangeCircles
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox DrawQ { get; private set; }
            public static CheckBox DrawW { get; private set; }
            public static CheckBox DrawE { get; private set; }
            public static CheckBox DrawR { get; private set; }

            static RangeCircles()
            {
                if (KillStealerFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("RangeCircles");
                    SubMenu.AddGroupLabel("RangeCircles Settings");

                    DrawQ = SubMenu.Add("drawQ", new CheckBox("Draw Q Range Circle", true));
                    DrawW = SubMenu.Add("drawW", new CheckBox("Draw W Range Circle", true));
                    DrawE = SubMenu.Add("drawE", new CheckBox("Draw E Range Circle", true));
                    DrawR = SubMenu.Add("drawR", new CheckBox("Draw R Range Circle", true));
                }
            }
        }

        public static class Combo
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }
            public static CheckBox UseW { get; private set; }
            public static CheckBox UseE { get; private set; }
            public static CheckBox UseR { get; private set; }

            static Combo()
            {
                if (ComboFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("Combo");
                    SubMenu.AddGroupLabel("Combo Settings");

                    UseQ = SubMenu.Add("comboUseQ", new CheckBox("Use Q", true));
                    UseW = SubMenu.Add("comboUseW", new CheckBox("Use W", true));
                    UseE = SubMenu.Add("comboUseE", new CheckBox("Use E", true));
                    UseR = SubMenu.Add("comboUseR", new CheckBox("Use R", true));
                }
            }
        }

        public static class Flee
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }
            public static CheckBox UseW { get; private set; }
            public static CheckBox UseE { get; private set; }
            public static CheckBox UseR { get; private set; }

            static Flee()
            {
                if (FleeFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("Flee");
                    SubMenu.AddGroupLabel("Flee Settings");

                    UseQ = SubMenu.Add("fleeUseQ", new CheckBox("Use Q", true));
                    UseW = SubMenu.Add("fleeUseW", new CheckBox("Use W", true));
                    UseE = SubMenu.Add("fleeUseE", new CheckBox("Use E", true));
                    UseR = SubMenu.Add("fleeUseR", new CheckBox("Use R", true));
                }
            }
        }

        public static class Harass
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }
            public static CheckBox UseW { get; private set; }
            public static CheckBox UseE { get; private set; }
            public static CheckBox UseR { get; private set; }

            static Harass()
            {
                if (HarassFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("Harass");
                    SubMenu.AddGroupLabel("Harass Settings");

                    UseQ = SubMenu.Add("harassUseQ", new CheckBox("Use Q", true));
                    UseW = SubMenu.Add("harassUseW", new CheckBox("Use W", true));
                    UseE = SubMenu.Add("harassUseE", new CheckBox("Use E", true));
                    UseR = SubMenu.Add("harassUseR", new CheckBox("Use R", true));
                }
            }
        }

        public static class JungleClear
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }
            public static CheckBox UseW { get; private set; }
            public static CheckBox UseE { get; private set; }
            public static CheckBox UseR { get; private set; }

            static JungleClear()
            {
                if (JungleClearFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("JungleCleaner");
                    SubMenu.AddGroupLabel("JungleCleaner Settings");

                    UseQ = SubMenu.Add("jungleUseQ", new CheckBox("Use Q", true));
                    UseW = SubMenu.Add("jungleUseW", new CheckBox("Use W", true));
                    UseE = SubMenu.Add("jungleUseE", new CheckBox("Use E", true));
                    UseR = SubMenu.Add("jungleUseR", new CheckBox("Use R", true));
                }
            }
        }

        public static class LaneClear
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }
            public static CheckBox UseW { get; private set; }
            public static CheckBox UseE { get; private set; }
            public static CheckBox UseR { get; private set; }

            static LaneClear()
            {
                if (LaneClearFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("LaneCleaner");
                    SubMenu.AddGroupLabel("LaneCleaner Settings");

                    UseQ = SubMenu.Add("laneUseQ", new CheckBox("Use Q", true));
                    UseW = SubMenu.Add("laneUseW", new CheckBox("Use W", true));
                    UseE = SubMenu.Add("laneUseE", new CheckBox("Use E", true));
                    UseR = SubMenu.Add("laneUseR", new CheckBox("Use R", true));
                }
            }
        }

        public static class KillStealer
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }
            public static CheckBox UseW { get; private set; }
            public static CheckBox UseE { get; private set; }
            public static CheckBox UseR { get; private set; }

            static KillStealer()
            {
                if (KillStealerFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("KillStealer");
                    SubMenu.AddGroupLabel("KillStealer Settings");

                    UseQ = SubMenu.Add("ksUseQ", new CheckBox("Use Q", true));
                    UseW = SubMenu.Add("ksUseW", new CheckBox("Use W", true));
                    UseE = SubMenu.Add("ksUseE", new CheckBox("Use E", true));
                    UseR = SubMenu.Add("ksUseR", new CheckBox("Use R", true));
                }
            }
        }
    }
}

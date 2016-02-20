using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK;
using EloBuddy;
using System;

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
        private const bool LastHitFunction = true;

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(AddonName, AddonName.ToLower());
            Menu.AddGroupLabel(string.Format("Welcome to {0}'s settings menu!\nYour feedback would be much appreciated!", AddonName));

            Combo.Init();
            Flee.Init();
            Harass.Init();
            JungleClear.Init();
            LaneClear.Init();
            LastHit.Init();
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

                    UseQ = SubMenu.Add("Use Q", new CheckBox("comboUseQ", true));
                    UseW = SubMenu.Add("Use W", new CheckBox("comboUseW", true));
                    UseE = SubMenu.Add("Use E", new CheckBox("comboUseE", true));
                    UseR = SubMenu.Add("Use R", new CheckBox("comboUseR", true));
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

                    UseQ = SubMenu.Add("Use Q", new CheckBox("comboUseQ", true));
                    UseW = SubMenu.Add("Use W", new CheckBox("comboUseW", true));
                    UseE = SubMenu.Add("Use E", new CheckBox("comboUseE", true));
                    UseR = SubMenu.Add("Use R", new CheckBox("comboUseR", true));
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

                    UseQ = SubMenu.Add("Use Q", new CheckBox("comboUseQ", true));
                    UseW = SubMenu.Add("Use W", new CheckBox("comboUseW", true));
                    UseE = SubMenu.Add("Use E", new CheckBox("comboUseE", true));
                    UseR = SubMenu.Add("Use R", new CheckBox("comboUseR", true));
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
                    SubMenu = Config.Menu.AddSubMenu("JungleClear");

                    UseQ = SubMenu.Add("Use Q", new CheckBox("comboUseQ", true));
                    UseW = SubMenu.Add("Use W", new CheckBox("comboUseW", true));
                    UseE = SubMenu.Add("Use E", new CheckBox("comboUseE", true));
                    UseR = SubMenu.Add("Use R", new CheckBox("comboUseR", true));
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
                    SubMenu = Config.Menu.AddSubMenu("LaneClear");

                    UseQ = SubMenu.Add("Use Q", new CheckBox("comboUseQ", true));
                    UseW = SubMenu.Add("Use W", new CheckBox("comboUseW", true));
                    UseE = SubMenu.Add("Use E", new CheckBox("comboUseE", true));
                    UseR = SubMenu.Add("Use R", new CheckBox("comboUseR", true));
                }
            }
        }

        public static class LastHit
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }
            public static CheckBox UseW { get; private set; }
            public static CheckBox UseE { get; private set; }
            public static CheckBox UseR { get; private set; }

            static LastHit()
            {
                if (LastHitFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("LastHit");

                    UseQ = SubMenu.Add("Use Q", new CheckBox("comboUseQ", true));
                    UseW = SubMenu.Add("Use W", new CheckBox("comboUseW", true));
                    UseE = SubMenu.Add("Use E", new CheckBox("comboUseE", true));
                    UseR = SubMenu.Add("Use R", new CheckBox("comboUseR", true));
                }
            }
        }
    }
}

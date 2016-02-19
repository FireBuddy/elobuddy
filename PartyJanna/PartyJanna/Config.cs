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
        public const string AddonName = "Party-Janna";

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
            Menu.AddGroupLabel(string.Format("Welcome to {0}'s configuration menu!", AddonName));

            Combo.Init();
            Flee.Init();
            Harass.Init();
            JungleClear.Init();
            LaneClear.Init();
            LastHit.Init();
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
                Q = new Spell.Skillshot(SpellSlot.Q, 1100, EloBuddy.SDK.Enumerations.SkillShotType.Circular);
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

            static Combo()
            {
                if (ComboFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("Combo");
                }
            }
        }

        public static class Flee
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            static Flee()
            {
                if (FleeFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("Flee");
                }
            }
        }

        public static class Harass
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            static Harass()
            {
                if (HarassFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("Harass");
                }
            }
        }

        public static class JungleClear
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            static JungleClear()
            {
                if (JungleClearFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("JungleClear");
                }
            }
        }

        public static class LaneClear
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            static LaneClear()
            {
                if (LaneClearFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("LaneClear");
                }
            }
        }

        public static class LastHit
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            static LastHit()
            {
                if (LastHitFunction)
                {
                    SubMenu = Config.Menu.AddSubMenu("LastHit");
                }
            }
        }
    }
}

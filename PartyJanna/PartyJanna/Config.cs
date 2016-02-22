using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;

namespace PartyJanna
{
    public static class Config
    {
        public static void Init()
        { }

        public const string AddonChampion = "Janna";
        public const string AddonName = "PartyJanna";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(AddonName, AddonName.ToLower());
            Menu.AddGroupLabel(string.Format("Welcome to {0}'s settings menu!\nYour feedback would be much appreciated!", AddonName));

            Spells.Init();
            Combo.Init();
            Draw.Init();
            Flee.Init();
            Harass.Init();
            LaneCleaner.Init();
            Protect.Init();
            RangeCircles.Init();
        }

        public static class Spells
        {
            public static void Init()
            { }

            public static Spell.Skillshot Q { get; private set; }
            public static Spell.Targeted W { get; private set; }
            public static Spell.Targeted E { get; private set; }
            public static Spell.Active R { get; private set; }

            public static int[] manaQ { get; private set; }
            public static int[] manaW { get; private set; }
            public static int[] manaE { get; private set; }
            public static int[] manaR { get; private set; }

            static Spells()
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Circular, 250, 900, 100);
                W = new Spell.Targeted(SpellSlot.W, 600);
                E = new Spell.Targeted(SpellSlot.E, 800);
                R = new Spell.Active(SpellSlot.R, 725);

                manaQ = new int[] { 90, 105, 120, 135, 150 };
                manaW = new int[] { 40, 50, 60, 70, 80 };
                manaE = new int[] { 70, 80, 90, 100, 110 };
                manaR = new int[] { 100, 100, 100, 100, 100 };
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
                SubMenu = Menu.AddSubMenu("RangeCircles");
                SubMenu.AddGroupLabel("RangeCircles Settings");

                SubMenu.AddSeparator();

                DrawQ = SubMenu.Add("drawQ", new CheckBox("Draw Q Range Circle", true));
                SubMenu.AddSeparator();
                DrawW = SubMenu.Add("drawW", new CheckBox("Draw W Range Circle", true));
                SubMenu.AddSeparator();
                DrawE = SubMenu.Add("drawE", new CheckBox("Draw E Range Circle", true));
                SubMenu.AddSeparator();
                DrawR = SubMenu.Add("drawR", new CheckBox("Draw R Range Circle", true));
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

            public static CheckBox IgnoreCollision { get; private set; }
            public static CheckBox TryToHitMultipleEnemies { get; private set; }

            public static Slider IgnoreCollisionEnemies { get; private set; }

            static Combo()
            {
                SubMenu = Menu.AddSubMenu("Combo");
                SubMenu.AddGroupLabel("Combo Settings");

                SubMenu.AddSeparator();

                UseQ = SubMenu.Add("comboUseQ", new CheckBox("Use Q", true));
                SubMenu.AddSeparator();
                UseW = SubMenu.Add("comboUseW", new CheckBox("Use W", true));
                SubMenu.AddSeparator();
                UseE = SubMenu.Add("comboUseE", new CheckBox("Use E to protect allies", false));

                SubMenu.AddSeparator(50);

                TryToHitMultipleEnemies = SubMenu.Add("hitMultipleEnemies", new CheckBox("Q Spell - Try to hit multiple enemies", false));
                SubMenu.AddSeparator();
                IgnoreCollision = SubMenu.Add("ignoreCollision", new CheckBox("Q Spell - Ignore Other Collisions (Not working very well)", false));

                SubMenu.AddSeparator();

                IgnoreCollisionEnemies = SubMenu.Add<Slider>("ignoreCollisionEnemies", new Slider("Enemies Nearby to Ignore Collisions:", 1, 1, 5));
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

            static Flee()
            {
                SubMenu = Menu.AddSubMenu("Flee");
                SubMenu.AddGroupLabel("Flee Settings");

                SubMenu.AddSeparator();

                UseQ = SubMenu.Add("fleeUseQ", new CheckBox("Use Q", true));
                SubMenu.AddSeparator();
                UseW = SubMenu.Add("fleeUseW", new CheckBox("Use W", true));
                SubMenu.AddSeparator();
                UseE = SubMenu.Add("fleeUseE", new CheckBox("Use E on yourself", true));
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

            public static CheckBox IgnoreCollision { get; private set; }
            public static CheckBox TryToHitMultipleEnemies { get; private set; }

            public static Slider IgnoreCollisionEnemies { get; private set; }

            static Harass()
            {
                SubMenu = Menu.AddSubMenu("Harass");
                SubMenu.AddGroupLabel("Harass Settings");

                SubMenu.AddSeparator();

                UseQ = SubMenu.Add("harassUseQ", new CheckBox("Use Q", false));
                SubMenu.AddSeparator();
                UseW = SubMenu.Add("harassUseW", new CheckBox("Use W", true));
                SubMenu.AddSeparator();
                UseE = SubMenu.Add("harassUseE", new CheckBox("Use E on yourself", true));

                SubMenu.AddSeparator(50);

                TryToHitMultipleEnemies = SubMenu.Add("hitMultipleEnemies", new CheckBox("Q Spell - Try to hit multiple enemies", false));
                SubMenu.AddSeparator();
                IgnoreCollision = SubMenu.Add("ignoreCollision", new CheckBox("Q Spell - Ignore Other Collisions (Not working very well)", false));

                SubMenu.AddSeparator();

                IgnoreCollisionEnemies = SubMenu.Add<Slider>("ignoreCollisionEnemies", new Slider("Enemies Nearby to Ignore Collisions:", 2, 1, 5));
            }
        }

        public static class LaneCleaner
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseQ { get; private set; }

            static LaneCleaner()
            {
                SubMenu = Menu.AddSubMenu("LaneCleaner");
                SubMenu.AddGroupLabel("LaneCleaner Settings");

                SubMenu.AddSeparator();

                UseQ = SubMenu.Add("laneUseQ", new CheckBox("Use Q", true));
            }
        }

        public static class Protect
        {
            public static void Init()
            { }

            private static readonly Menu SubMenu;

            public static CheckBox UseE { get; private set; }

            public static ComboBox PriorityMode { get; private set; }

            public static List<Slider> SliderList { get; private set; }
            public static List<AIHeroClient> AIHeroClientList { get; private set; }


            static Protect()
            {
                SliderList = new List<Slider>();
                AIHeroClientList = new List<AIHeroClient>();

                SubMenu = Menu.AddSubMenu("Protect");
                SubMenu.AddGroupLabel("Protect Settings");

                SubMenu.AddSeparator();

                UseE = SubMenu.Add("protectUseE", new CheckBox("Use E", true));

                SubMenu.AddSeparator(50);

                SubMenu.AddGroupLabel("Protection Priorities");

                SubMenu.AddSeparator();

                PriorityMode = SubMenu.Add<ComboBox>("priorityMode", new ComboBox("Protect by:", 0, new string[] { "Lowest Health", "Priority Level" }));

                SubMenu.AddSeparator();

                foreach (AIHeroClient Ally in EntityManager.Heroes.Allies)
                {
                    if (Ally.ChampionName != AddonChampion)
                    {
                        Slider PrioritySlider = SubMenu.Add<Slider>(Ally.ChampionName, new Slider(string.Format("{0} ({1})", Ally.ChampionName, Ally.Name), 1, 1, EntityManager.Heroes.Allies.Count - 1));

                        SubMenu.AddSeparator(13);

                        SliderList.Add(PrioritySlider);

                        AIHeroClientList.Add(Ally);
                    }
                }
            }
        }
    }
}

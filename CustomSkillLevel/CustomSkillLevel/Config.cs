using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.IO;

namespace CustomSkillLevel
{
    class Config
    {
        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu("CustomSkillLevel", "csl");
            Menu.AddGroupLabel("Welcome to CustomSkillLevel!");

            CSL.Initialize();
        }

        public static void Initialize() { }

        public static class CSL
        {
            private static readonly Menu Menu;

            static CSL()
            {
                Menu = Config.Menu.AddSubMenu("Settings");

                LevelingOrderMenu.Initialize();
                Menu.AddSeparator();
            }

            public static void Initialize() { }

            public static class LevelingOrderMenu
            {
                public static readonly List<ComboBox> orderBox = new List<ComboBox>();
                public static readonly CheckBox saveButton, rndmDelay, enabled;
                public static readonly Slider delay;

                static LevelingOrderMenu()
                {
                    Menu.AddGroupLabel("CSL Settings");

                    enabled = Menu.Add("enabled", new CheckBox("Enabled"));

                    Menu.AddSeparator(13);

                    delay = Menu.Add("delay", new Slider("Delay to Evolve Abilities (1 sec = 1000 ms):", 500, 0, 2000));

                    Menu.AddSeparator(13);

                    rndmDelay = Menu.Add("rndmdelay", new CheckBox("Randomize Evolve Delay"));

                    Menu.AddSeparator(13);

                    Random randomId = new Random();

                    int level = 1;

                    foreach (string slot in Program.order)
                    {
                        orderBox.Add(Menu.Add((randomId.Next() + level).ToString(), new ComboBox("Level " + level.ToString(), ConvertToMenuIndex(slot), new string[] { "Q", "W", "E", "R", "None" })));

                        level++;

                        if (level > 18)
                            break;
                    }

                    saveButton = Menu.Add("savebutton", new CheckBox("Check Me to Save Your Settings", false));

                    saveButton.OnValueChange += onSaveRequest;
                }

                private static void onSaveRequest(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    if (args.NewValue)
                    {
                        File.Delete(Program.cslpath + Player.Instance.ChampionName + ".txt");

                        using (StreamWriter streamWriter = new StreamWriter(Program.cslpath + Player.Instance.ChampionName + ".txt"))
                        {
                            foreach (ComboBox comboBox in orderBox)
                            {
                                streamWriter.Write(Program.ConvertToSlot(comboBox.SelectedIndex).ToString() + ", ");
                            }

                            streamWriter.Close();
                        }

                        saveButton.DisplayName = "Check Me to Save Your Settings [SAVED]";
                        saveButton.CurrentValue = false;
                    }
                }

                private static int ConvertToMenuIndex(string slot)
                {
                    switch (slot)
                    {
                        case "Q": { return 0; }
                        case "W": { return 1; }
                        case "E": { return 2; }
                        case "R": { return 3; }
                        default: { return 4; }
                    }
                }

                public static void Initialize() { }
            }
        }
    }
}

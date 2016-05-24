using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.IO;

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
                public static readonly List<ComboBox> buyingOrderBoxes = new List<ComboBox>();
                public static readonly CheckBox /*saveButton, */rndmDelay, active;
                public static readonly Slider delay;

                static BuyingOrderMenu()
                {
                    Menu.AddGroupLabel("CSL Settings");

                    active = Menu.Add("active", new CheckBox("Activated"));

                    Menu.AddSeparator(13);

                    delay = Menu.Add("delay", new Slider("Delay to Buy Items (1sec = 1000ms):", 343, 0, 2000));

                    Menu.AddSeparator(13);

                    rndmDelay = Menu.Add("rndmdelay", new CheckBox("Randomize Delay?"));

                    Menu.AddSeparator(13);

                    /*Random randomId = new Random();

                    int level = 1;

                    foreach (string slot in Program.buyingOrder)
                    {
                        levelingOrderBoxes.Add(Menu.Add((randomId.Next() + level).ToString(), new ComboBox("Level " + level.ToString(), ConvertToMenuIndex(slot), new string[] { "Q", "W", "E", "R", "None" })));

                        level++;

                        if (level > 18)
                            break;
                    }

                    saveButton = Menu.Add("savebutton", new CheckBox("Check Me to Save Your Settings", false));

                    saveButton.OnValueChange += onSaveRequest;*/
                }

                /*private static void onSaveRequest(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
                {
                    if (args.NewValue)
                    {
                        File.Delete(Program.appDataPath + @"\EloBuddy\CSL\" + Player.Instance.ChampionName + ".txt");

                        using (StreamWriter streamWriter = new StreamWriter(Program.appDataPath + @"\EloBuddy\CSL\" + Player.Instance.ChampionName + ".txt"))
                        {
                            foreach (ComboBox comboBox in levelingOrderBoxes)
                            {
                                streamWriter.Write(Program.ConvertToSlot(comboBox.SelectedIndex).ToString() + ", ");
                            }

                            streamWriter.Close();
                        }

                        saveButton.DisplayName = "Check Me to Save Your Settings [SAVED]";
                        saveButton.CurrentValue = false;
                    }
                }*/

                /*private static int ConvertToMenuIndex(string slot)
                {
                    switch (slot)
                    {
                        case "Q": { return 0; }
                        case "W": { return 1; }
                        case "E": { return 2; }
                        case "R": { return 3; }
                        default: { return 4; }
                    }
                }*/

                public static void Initialize() { }
            }
        }
    }
}

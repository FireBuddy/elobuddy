using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Settings = CustomSkillLevel.Config.CSL.LevelingOrderMenu;

namespace CustomSkillLevel
{
    public static class Program
    {
        public static List<string> levelingOrder { get; private set; }
        public static string appDataPath;
        public static int lastLevel = 0;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            try
            {
                Chat.Print("CustomSkillLevel by houzeparty");

                lastLevel = Player.Instance.Level - Player.Instance.SpellTrainingPoints;

                appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                Directory.CreateDirectory(appDataPath + @"\EloBuddy\CSL");

                if (!File.Exists(appDataPath + @"\EloBuddy\CSL\" + Player.Instance.ChampionName + ".txt"))
                {
                    using (var streamWriter = new StreamWriter(appDataPath + @"\EloBuddy\CSL\" + Player.Instance.ChampionName + ".txt"))
                    {
                        streamWriter.Write("None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None");
                        streamWriter.Close();
                    }
                }

                using (var streamReader = new StreamReader(appDataPath + @"\EloBuddy\CSL\" + Player.Instance.ChampionName + ".txt"))
                {
                    levelingOrder = streamReader.ReadLine().ToUpper().Split(',').Select(str => str.Trim()).ToList();

                    streamReader.Close();
                }

                Config.Initialize();

                Game.OnTick += OnTick;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnTick(EventArgs args)
        {
            if (!Settings.active.CurrentValue)
                return;

            if (Player.Instance.Level > lastLevel)
            {
                SpellSlot levelSlot = ConvertToSlot(Settings.levelingOrderBoxes[lastLevel].CurrentValue);

                if (levelSlot != SpellSlot.Unknown)
                    EloBuddy.SDK.Core.DelayAction(() => { Player.LevelSpell(levelSlot); }, Settings.rndmDelay.CurrentValue ? new Random().Next(0, Settings.delay.CurrentValue) : Settings.delay.CurrentValue);

                lastLevel++;
            }
        }

        public static SpellSlot ConvertToSlot(int index)
        {
            switch (index)
            {
                case 0: { return SpellSlot.Q; }
                case 1: { return SpellSlot.W; }
                case 2: { return SpellSlot.E; }
                case 3: { return SpellSlot.R; }
                default: { return SpellSlot.Unknown; }
            }
        }
    }
}

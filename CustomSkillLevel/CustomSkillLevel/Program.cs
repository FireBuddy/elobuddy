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
        public static List<string> order { get; private set; }
        public static string cslpath;
        public static int last = 0;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            try
            {
                Chat.Print("CustomSkillLevel by houzeparty");

                last = Player.Instance.Level - Player.Instance.SpellTrainingPoints;

                cslpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\EloBuddy\CSL";

                Directory.CreateDirectory(cslpath);

                if (!File.Exists(cslpath + Player.Instance.ChampionName + ".txt"))
                {
                    using (var streamWriter = new StreamWriter(cslpath + Player.Instance.ChampionName + ".txt"))
                    {
                        streamWriter.Write("None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None, None");
                        streamWriter.Close();
                    }
                }

                using (var streamReader = new StreamReader(cslpath + Player.Instance.ChampionName + ".txt"))
                {
                    order = streamReader.ReadLine().ToUpper().Split(',').Select(str => str.Trim()).ToList();

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
            if (!Settings.enabled.CurrentValue)
                return;

            if (Player.Instance.Level > last)
            {
                SpellSlot levelSlot = ConvertToSlot(Settings.orderBox[last].CurrentValue);

                if (levelSlot != SpellSlot.Unknown)
                    EloBuddy.SDK.Core.DelayAction(() => { Player.LevelSpell(levelSlot); }, Settings.rndmDelay.CurrentValue ? new Random().Next(0, Settings.delay.CurrentValue) : Settings.delay.CurrentValue);

                last++;
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

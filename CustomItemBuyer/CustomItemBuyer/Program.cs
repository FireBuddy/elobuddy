using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Settings = CustomItemBuyer.Config.CIB.BuyingOrderMenu;

namespace CustomItemBuyer
{
    public static class Program
    {
        public static List<string> buyingOrder { get; private set; }
        public static string appDataPath;
        public static int lastItem = 0;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            try
            {
                Chat.Print("CustomItemBuyer by houzeparty");

                lastItem = 0;

                appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                Directory.CreateDirectory(appDataPath + @"\EloBuddy\CIB");

                if (!File.Exists(appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt"))
                {
                    File.Create(appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt");
                }

                using (var streamReader = new StreamReader(appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt"))
                {
                    buyingOrder = streamReader.ReadLine().ToUpper().Split(',').Select(str => str.Trim()).ToList();

                    streamReader.Close();
                }

                Config.Initialize();

                Player.OnSpawn += OnSpawn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnSpawn(Obj_AI_Base sender)
        {
            Chat.Print("OnSpawn Event was Triggered!");
        }

        /*private static void OnTick(EventArgs args)
        {
            if (!Settings.active.CurrentValue)
                return;

            if (Player.)
            {
                SpellSlot levelSlot = ConvertToSlot(Settings.levelingOrderBoxes[lastItem].CurrentValue);

                if (levelSlot != SpellSlot.Unknown)
                    EloBuddy.SDK.Core.DelayAction(() => { Player.LevelSpell(levelSlot); }, Settings.rndmDelay.CurrentValue ? new Random().Next(0, Settings.delay.CurrentValue) : Settings.delay.CurrentValue);

                lastItem++;
            }
        }*/

        /*public static SpellSlot ConvertToSlot(int index)
        {
            switch (index)
            {
                case 0: { return SpellSlot.Q; }
                case 1: { return SpellSlot.W; }
                case 2: { return SpellSlot.E; }
                case 3: { return SpellSlot.R; }
                default: { return SpellSlot.Unknown; }
            }
        }*/
    }
}

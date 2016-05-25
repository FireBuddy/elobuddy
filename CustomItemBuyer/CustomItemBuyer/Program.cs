using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Color = System.Drawing.Color;
using Settings = CustomItemBuyer.Config.CIB.BuyingOrderMenu;

namespace CustomItemBuyer
{
    public static class Program
    {
        private static List<int> idOrder;
        private static List<Item> itemOrder;
        private static string appDataPath;
        private static Item nextItem;
        private static Text priceText;

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            try
            {
                Chat.Print("CustomItemBuyer by houzeparty");

                priceText = new Text("", new Font("Consolas", 11f, FontStyle.Bold));

                appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                Directory.CreateDirectory(appDataPath + @"\EloBuddy\CIB");

                if (!File.Exists(appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt"))
                    File.Create(appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt");

                try
                {
                    using (var streamReader = new StreamReader(appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt"))
                    {
                        idOrder = streamReader.ReadToEnd().Split(',').Select(str => str.Trim()).ToList().ConvertAll(Convert.ToInt32);

                        foreach (var itemId in idOrder)
                            itemOrder.Add(new Item(itemId));

                        streamReader.Close();
                    }
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("CustomItemBuyer: There are no items set for {0} at \"{1}\"", Player.Instance.ChampionName, appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt");
                    Chat.Print("CustomItemBuyer: There are no items set for {0} at \"{1}\"", Player.Instance.ChampionName, appDataPath + @"\EloBuddy\CIB\" + Player.Instance.ChampionName + ".txt");
                }

                Config.Initialize();

                AIHeroClient.OnBuffGain += OnBuffGain;

                Drawing.OnDraw += OnDraw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe || args.Buff.Name != "srhomeguardspeed")
                return;

            for (int i = 0; i < itemOrder.Count; i++)
            {
                if (!itemOrder[i].IsOwned(Player.Instance))
                {
                    nextItem = itemOrder[i];
                    break;
                }
            }

            if (Player.Instance.Gold >= nextItem.GoldRequired())
                Core.DelayAction(() => { nextItem.Buy(); }, Settings.rndmDelay.CurrentValue ? new Random().Next(0, Settings.delay.CurrentValue) : Settings.delay.CurrentValue);
        }

        private static void OnDraw(EventArgs args)
        {
            priceText.TextValue = Settings.enabled.CurrentValue ? "Next Item Price: " + nextItem.GoldRequired().ToString() : "";
            priceText.Color = Player.Instance.Gold >= nextItem.GoldRequired() ? Color.Green : Color.Red;
            priceText.Position = Player.Instance.Position.WorldToScreen() - new Vector2(priceText.Bounding.Width / 2, -75);
        }
    }
}

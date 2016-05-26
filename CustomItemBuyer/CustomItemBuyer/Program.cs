using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Color = System.Drawing.Color;
using Settings = CustomItemBuyer.Config.CIB.BuyingOrderMenu;

namespace CustomItemBuyer
{
    public static class Program
    {
        private static Stopwatch stopwatch;

        private static List<int> ids;

        private static List<Item> order;

        private static int current;

        private static string cibpath;

        private static Item hppot, bisc, bo, bom, bos, bol, wt, gst, gvt, oa, sl, ss, sa;

        private static Text text = new Text("", new Font("Consolas", 11f));

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            try
            {
                stopwatch = new Stopwatch();
                ids = new List<int>();
                order = new List<Item>();
                hppot = new Item(2003);
                bisc = new Item(2010);
                bo = new Item(1001);
                bom = new Item(3117);
                bos = new Item(3009);
                bol = new Item(3158);
                wt = new Item(3340);
                gst = new Item(3361);
                gvt = new Item(3362);
                ss = new Item(3462);
                sa = new Item(3345);
                oa = new Item(3364);
                sl = new Item(3341);

                Chat.Print("CustomItemBuyer by houzeparty");

                cibpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\EloBuddy\CIB\";

                if (!File.Exists(cibpath + Player.Instance.ChampionName + ".txt"))
                    File.Create(cibpath + Player.Instance.ChampionName + ".txt");

                if (!File.Exists(cibpath + @"dontTouchMe.txt") || GetQtt(wt) == 0 || GetQtt(gst) == 0 || GetQtt(gvt) == 0 || GetQtt(ss) == 0 || GetQtt(sa) == 0 || GetQtt(oa) == 0 || GetQtt(sl) == 0)
                {
                    using (var sw = new StreamWriter(cibpath + @"dontTouchMe.txt", false))
                    {
                        sw.Write("0");
                        sw.Close();
                    }
                }

                using (var sr = new StreamReader(cibpath + @"dontTouchMe.txt"))
                {
                    current = Convert.ToInt32(sr.ReadToEnd());
                }

                try
                {
                    using (var streamReader = new StreamReader(cibpath + Player.Instance.ChampionName + ".txt"))
                    {
                        string settings = streamReader.ReadToEnd();

                        ids = settings.Split(',').Select(str => str.Trim()).ToList().ConvertAll(Convert.ToInt32);

                        streamReader.Close();
                    }
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("CustomItemBuyer: There are no items set for {0} at \"{1}\"", Player.Instance.ChampionName, cibpath + Player.Instance.ChampionName + ".txt");
                    Chat.Print("CustomItemBuyer: There are no items set for {0} at \"{1}\"", Player.Instance.ChampionName, cibpath + Player.Instance.ChampionName + ".txt");
                }

                foreach (var id in ids)
                    order.Add(new Item(id));

                Config.Initialize();

                stopwatch.Start();

                Game.OnTick += OnTick;

                Drawing.OnDraw += OnDraw;
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

            if (current + 1 > order.Count)
                return;

            text.Color = Player.Instance.Gold >= order[current].GoldRequired() ? Color.LightGreen : Color.DarkRed;
            text.TextValue = Settings.enabled.CurrentValue ? string.Format("Next Item: {0}\nPrice: {1}", order[current].ItemInfo.Name, order[current].GoldRequired().ToString()) : string.Empty;

            if (Settings.rndmDelay.CurrentValue)
            {
                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Settings.delay.CurrentValue))
                {
                    if (Player.Instance.IsInShopRange() && Player.Instance.Gold >= order[current].GoldRequired())
                    {
                        order[current].Buy();

                        current += 1;

                        SaveCurrent();
                    }

                    stopwatch.Restart();
                }
            }
            else
            {
                if (stopwatch.ElapsedMilliseconds >= Settings.delay.CurrentValue)
                {
                    if (Player.Instance.IsInShopRange() && Player.Instance.Gold >= order[current].GoldRequired())
                    {
                        order[current].Buy();

                        current += 1;

                        SaveCurrent();
                    }

                    stopwatch.Restart();
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (!Settings.enabled.CurrentValue || !Settings.draw.CurrentValue)
                return;

            text.Position = Player.Instance.Position.WorldToScreen() - new Vector2(text.Bounding.Width / 2, -75);
            text.Draw();
        }

        private static void SaveCurrent()
        {
            using (var sw = new StreamWriter(cibpath + @"dontTouchMe.txt", false))
            {
                sw.Write(current);
                sw.Close();
            }
        }

        public static int GetQtt(Item item)
        {
            int qtt = 0;

            foreach (var slot in Player.Instance.InventoryItems)
            {
                if (slot.Id == item.Id)
                    qtt++;
            }

            return qtt;
        }

        public static InventorySlot GetSlot(Item item)
        {
            foreach (var slot in Player.Instance.InventoryItems)
            {
                if (slot.Id == item.Id)
                    return slot;
            }

            return null;
        }
    }
}

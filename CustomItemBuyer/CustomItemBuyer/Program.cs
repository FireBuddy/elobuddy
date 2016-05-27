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

        private static List<char> op;

        private static List<Item> order;

        private static int current, goldReq, compTotal;

        private static string cibpath;

        private static Item hppot, bisc, bo, bom, bos, bol, wt, gst, gvt, oa, sl, ss, sa, dps;

        private static Text text;

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
                dps = new Item(2054);

                text = new Text("[Next Item Info]", new Font("Consolas", 12f));

                Chat.Print("CustomItemBuyer by houzeparty");

                cibpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\EloBuddy\CIB\";

                if (!File.Exists(cibpath + Player.Instance.ChampionName + ".txt"))
                    File.Create(cibpath + Player.Instance.ChampionName + ".txt");

                Game.OnNotify += OnNotify;

                /*if (!File.Exists(cibpath + @"saved_data.txt"))
                {
                    using (var sw = new StreamWriter(cibpath + @"saved_data.txt", false))
                    {
                        sw.Write("0:0");
                        sw.Close();
                    }
                }*/

                /*if (!wt.IsOwned() && !gst.IsOwned() && !gvt.IsOwned() && !ss.IsOwned() && !sa.IsOwned() && !oa.IsOwned() && !sl.IsOwned())
                {
                    using (var sw = new StreamWriter(cibpath + @"saved_data.txt", false))
                    {
                        sw.Write("0:0");
                        sw.Close();
                    }
                }*/

                using (var sr = new StreamReader(cibpath + @"saved_data.txt"))
                {
                    string settings = sr.ReadToEnd();

                    current = Convert.ToInt32(settings.Substring(0, settings.IndexOf(':')));

                    goldReq = Convert.ToInt32(settings.Substring(settings.IndexOf(':') + 1));
                }

                try
                {
                    using (var sr = new StreamReader(cibpath + Player.Instance.ChampionName + ".txt"))
                    {
                        string settings = sr.ReadToEnd();

                        ids = settings.Split(',').Select(str => str.Substring(str.IndexOf(':') + 1).Trim()).ToList().ConvertAll(Convert.ToInt32);

                        op = settings.Split(',').Select(str => str.Substring(0, str.IndexOf(':')).Trim().ToLower()).ToList().ConvertAll(Convert.ToChar);

                        sr.Close();
                    }
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("CustomItemBuyer: There are no items set for {0} at \"{1}\"", Player.Instance.ChampionName, cibpath + Player.Instance.ChampionName + ".txt");
                    Chat.Print("CustomItemBuyer: There are no items set for {0} at \"{1}\"", Player.Instance.ChampionName, cibpath + Player.Instance.ChampionName + ".txt");
                }

                foreach (var id in ids)
                    order.Add(new Item(id));

                stopwatch.Start();

                Config.Initialize();

                Game.OnTick += OnTick;

                Drawing.OnDraw += OnDraw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void OnNotify(GameNotifyEventArgs args)
        {
            if (args.EventId == GameEventId.OnGameStart)
            {
                using (var sw = new StreamWriter(cibpath + @"saved_data.txt", false))
                {
                    sw.Write("0:0");
                    sw.Close();
                }
            }
        }

        private static void OnTick(EventArgs args)
        {
            if (!Settings.enabled.CurrentValue)
                return;

            if (current + 1 > order.Count)
                return;

            goldReq = order[current].GoldRequired();

            text.Color = Player.Instance.Gold >= goldReq ? Color.LightGreen : Color.DarkRed;
            text.TextValue = Settings.enabled.CurrentValue ? string.Format("Next Item: {0}\nPrice: {1}", order[current].ItemInfo.Name, goldReq) : string.Empty;

            if (Settings.rndmDelay.CurrentValue)
            {
                if (stopwatch.ElapsedMilliseconds >= new Random().Next(250, Settings.delay.CurrentValue))
                {
                    if (Player.Instance.IsInShopRange())
                    {
                        TryBuySell(order[current], op[current]);

                        SaveData();
                    }

                    stopwatch.Restart();
                }
            }
            else
            {
                if (stopwatch.ElapsedMilliseconds >= Settings.delay.CurrentValue)
                {
                    if (Player.Instance.IsInShopRange())
                    {
                        TryBuySell(order[current], op[current]);

                        SaveData();
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

        private static void SaveData()
        {
            using (var sw = new StreamWriter(cibpath + @"saved_data.txt", false))
            {
                sw.Write("{0}:{1}", current, goldReq);
                sw.Close();
            }
        }

        private static int GetQtt(Item item)
        {
            int qtt = 0;

            foreach (var slot in Player.Instance.InventoryItems)
            {
                if (slot.Id == item.Id)
                    qtt++;
            }

            return qtt;
        }

        private static InventorySlot GetSlot(Item item)
        {
            foreach (var slot in Player.Instance.InventoryItems)
            {
                if (slot.Id == item.Id)
                    return slot;
            }

            return null;
        }

        private static void TryBuySell(Item item, char bs)
        {
            switch (bs)
            {
                case 'b':
                    {
                        if (Player.Instance.Gold >= goldReq)
                        {
                            Shop.BuyItem(item.Id);

                            current++;
                        }
                        /*else if (Settings.buyComp.CurrentValue)
                        {
                            foreach (var comp1 in item.GetComponents().OrderByDescending(x => x.GoldRequired()))
                            {
                                if (Shop.BuyItem(comp1.Id))
                                    goldReq -= comp1.ItemInfo.Gold.Base;
                            }
                        }*/
                    }
                    break;
                case 's':
                    {
                        if (item.Id == hppot.Id)
                        {
                            if (hppot.IsOwned())
                            {
                                Shop.SellItem(GetSlot(hppot).Slot);

                                current++;

                                goldReq = order[current].ItemInfo.Gold.Total;
                            }
                            else if (bisc.IsOwned())
                            {
                                Shop.SellItem(GetSlot(bisc).Slot);

                                current++;

                                goldReq = order[current].ItemInfo.Gold.Total;
                            }
                        }
                        else if (item.IsOwned())
                        {
                            Shop.SellItem(GetSlot(item).Slot);

                            current++;

                            goldReq = order[current].ItemInfo.Gold.Total;
                        }
                    }
                    break;
                default:
                    {
                        Console.WriteLine("Error: item set for {0} was not set properly - b:ItemId or s:ItemId", Player.Instance.ChampionName);
                    }
                    break;
            }
        }
    }
}

using EloBuddy;
using EloBuddy.SDK;
using System.Diagnostics;
using System.Linq;
using AutoShield = PartyMorg.Config.Settings.AutoShield;
using Settings = PartyMorg.Config.Settings.Items;

namespace PartyMorg.Modes
{
    public class PermaActive : ModeBase
    {
        private Item ironSolari = new Item(3190, 600);
        private Item mountain = new Item(3401, 600);
        private Item mikael = new Item(3222, 750);
        private Item frostQueen = new Item(3092, 4500);
        private Item talisman = new Item(3069, 600);

        private static Stopwatch stopwatch = new Stopwatch();

        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public void JannaStop()
        {
            Player.IssueOrder(GameObjectOrder.Stop, Player.Instance.Position);
        }

        public override void Execute()
        {
            if (!Settings.UseItems || Player.Instance.CountEnemiesInRange(2200) == 0 || Player.Instance.IsRecalling())
            { return; }

            if (Settings.UseItemsComboOnly && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            { return; }

            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (ally.IsFacing(enemy))
                    {
                        foreach (var itemOnThisAlly in Settings.MCAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue && ally.HealthPercent <= Settings.AllyHpPercentageCC && ally.IsInRange(Player.Instance, 750)))
                        {
                            if (mikael.IsOwned() && mikael.IsReady() && (ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Poison) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Slow) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt)))
                            {
                                mikael.Cast(ally);
                            }
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage && ally.IsInRange(Player.Instance, 600))
                        {
                            foreach (var itemOnThisAlly in Settings.ISAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (ironSolari.IsOwned() && ironSolari.IsReady())
                                {
                                    ironSolari.Cast();
                                }
                            }

                            foreach (var itemOnThisAlly in Settings.FOTMAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (mountain.IsOwned() && mountain.IsReady())
                                {
                                    mountain.Cast(ally);
                                }
                            }
                        }

                        if (enemy.HealthPercent <= Settings.AllyHpPercentageDamage && enemy.IsInRange(Player.Instance, 2200))
                        {
                            foreach (var itemOnThisAlly in Settings.TOAAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue && ally.IsInRange(Player.Instance, 600)))
                            {
                                if (talisman.IsOwned() && talisman.IsReady())
                                {
                                    talisman.Cast();
                                }
                            }

                            foreach (var itemOnThisAlly in Settings.FQCAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (frostQueen.IsOwned() && frostQueen.IsReady())
                                {
                                    frostQueen.Cast(enemy);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var itemOnThisAlly in Settings.MCAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue && ally.HealthPercent <= Settings.AllyHpPercentageCC && ally.IsInRange(Player.Instance, 750)))
                        {
                            if (mikael.IsOwned() && mikael.IsReady() && (ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Poison) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Slow) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt)))
                            {
                                mikael.Cast(ally);
                            }
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage && ally.IsInRange(Player.Instance, 600))
                        {
                            foreach (var itemOnThisAlly in Settings.ISAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (ironSolari.IsOwned() && ironSolari.IsReady())
                                {
                                    ironSolari.Cast();
                                }
                            }

                            foreach (var itemOnThisAlly in Settings.FOTMAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (mountain.IsOwned() && mountain.IsReady())
                                {
                                    mountain.Cast(ally);
                                }
                            }
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage && enemy.IsInRange(Player.Instance, 1650))
                        {
                            foreach (var itemOnThisAlly in Settings.TOAAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue && ally.IsInRange(Player.Instance, 600)))
                            {
                                if (talisman.IsOwned() && talisman.IsReady())
                                {
                                    talisman.Cast();
                                }
                            }

                            foreach (var itemOnThisAlly in Settings.FQCAllyList.Where(x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue))
                            {
                                if (frostQueen.IsOwned() && frostQueen.IsReady())
                                {
                                    frostQueen.Cast(enemy);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

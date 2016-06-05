using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyMorg.Config.Settings.Items;

namespace PartyMorg.Modes
{
    public class PermaActive : ModeBase
    {
        private readonly Item frostQueen = new Item(3092, 4500);
        private readonly Item ironSolari = new Item(3190, 600);
        private readonly Item mikael = new Item(3222, 750);
        private readonly Item mountain = new Item(3401, 600);
        private readonly Item talisman = new Item(3069, 600);

        public override bool ShouldBeExecuted() => true;

        public override void Execute()
        {
            if (!Settings.UseItems || Player.Instance.CountEnemiesInRange(2200) == 0 || Player.Instance.IsRecalling()) return;

            if (Settings.UseItemsComboOnly && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;

            foreach (var enemy in EntityManager.Heroes.Enemies)
                foreach (var ally in EntityManager.Heroes.Allies)
                    if (ally.IsFacing(enemy))
                    {
                        foreach (var itemOnThisAlly in Settings.MCAllyList.Where(
                            x =>
                                x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                ally.HealthPercent <= Settings.AllyHpPercentageCC &&
                                ally.IsInRange(Player.Instance, 750))
                            .Where(itemOnThisAlly => mikael.IsOwned() && mikael.IsReady() &&
                                                     HasDebuff(ally)))
                        {
                            mikael.Cast(ally);
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage &&
                            ally.IsInRange(Player.Instance, 600))
                        {
                            foreach (var itemOnThisAlly in Settings.ISAllyList.Where(
                                x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                .Where(itemOnThisAlly => ironSolari.IsOwned() && ironSolari.IsReady()))
                            {
                                ironSolari.Cast();
                            }

                            foreach (var itemOnThisAlly in Settings.FOTMAllyList.Where(
                                x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                .Where(itemOnThisAlly => mountain.IsOwned() && mountain.IsReady()))
                            {
                                mountain.Cast(ally);
                            }
                        }

                        if (!(enemy.HealthPercent <= Settings.AllyHpPercentageDamage) ||
                            !enemy.IsInRange(Player.Instance, 2200)) continue;

                        foreach (var itemOnThisAlly in Settings.TOAAllyList.Where(
                            x =>
                                x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                ally.IsInRange(Player.Instance, 600))
                            .Where(itemOnThisAlly => talisman.IsOwned() && talisman.IsReady()))
                        {
                            talisman.Cast();
                        }

                        foreach (var itemOnThisAlly in Settings.FQCAllyList.Where(
                            x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                            .Where(itemOnThisAlly => frostQueen.IsOwned() && frostQueen.IsReady()))
                        {
                            frostQueen.Cast(enemy);
                        }
                    }
                    else
                    {
                        foreach (var itemOnThisAlly in Settings.MCAllyList.Where(
                            x =>
                                x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                ally.HealthPercent <= Settings.AllyHpPercentageCC &&
                                ally.IsInRange(Player.Instance, 750))
                            .Where(itemOnThisAlly => mikael.IsOwned() && mikael.IsReady() &&
                                                     HasDebuff(ally)))
                        {
                            mikael.Cast(ally);
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage &&
                            ally.IsInRange(Player.Instance, 600))
                        {
                            foreach (var itemOnThisAlly in Settings.ISAllyList.Where(
                                x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                .Where(itemOnThisAlly => ironSolari.IsOwned() && ironSolari.IsReady()))
                            {
                                ironSolari.Cast();
                            }

                            foreach (var itemOnThisAlly in Settings.FOTMAllyList.Where(
                                x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                .Where(itemOnThisAlly => mountain.IsOwned() && mountain.IsReady()))
                            {
                                mountain.Cast(ally);
                            }
                        }

                        if (!(ally.HealthPercent <= Settings.AllyHpPercentageDamage) ||
                            !enemy.IsInRange(Player.Instance, 1650)) continue;

                        foreach (var itemOnThisAlly in Settings.TOAAllyList.Where(
                            x =>
                                x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                ally.IsInRange(Player.Instance, 600))
                            .Where(itemOnThisAlly => talisman.IsOwned() && talisman.IsReady()))
                        {
                            talisman.Cast();
                        }

                        foreach (var itemOnThisAlly in Settings.FQCAllyList.Where(
                            x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                            .Where(itemOnThisAlly => frostQueen.IsOwned() && frostQueen.IsReady()))
                        {
                            frostQueen.Cast(enemy);
                        }
                    }
        }
    }
}
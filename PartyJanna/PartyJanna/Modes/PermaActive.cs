using System;
using System.Diagnostics;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyJanna.Config.Settings.Items;

namespace PartyJanna.Modes
{
    public class PermaActive : ModeBase
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Item frostQueen = new Item(3092, 4500);
        private readonly Item ironSolari = new Item(3190, 600);
        private readonly Item mikael = new Item(3222, 750);
        private readonly Item mountain = new Item(3401, 600);
        private readonly Item talisman = new Item(3069, 600);

        public override bool ShouldBeExecuted() => true;

        private static void JannaStop() => Player.IssueOrder(GameObjectOrder.Stop, Player.Instance.Position);

        public override void Execute()
        {
            if (Config.Settings.Harass.AutoHarass &&
                Player.Instance.ManaPercent >= Config.Settings.Harass.AutoHarassManaPercent)
            {
                var target = GetTarget(W, DamageType.Magical);

                if (target != null)
                    W.Cast(target);
            }

            if (!Settings.UseItems || Player.Instance.CountEnemiesInRange(2200) == 0 || Player.Instance.IsRecalling())
                return;

            if (Settings.UseItemsComboOnly && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;

            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (R.IsLearned && R.IsReady() && Config.Settings.AutoShield.AutoUltimate)
                    {
                        foreach (
                            var js in
                                from ultThisAlly in
                                    Config.Settings.AutoShield.UltAllyList.Where(
                                        a =>
                                            a.DisplayName.Contains(ally.ChampionName) &&
                                            Player.Instance.IsInRange(ally, R.Range) && a.CurrentValue)
                                from slider in Config.Settings.AutoShield.UltSliders
                                where ally.HealthPercent <= slider.CurrentValue
                                select new Action(JannaStop))
                        {
                            R.Cast();

                            stopwatch.Start();

                            do
                                Core.RepeatAction(js, 252, 0); while (stopwatch.ElapsedMilliseconds <= 3000 &&
                                                                      !IsImmobile(Player.Instance));

                            stopwatch.Reset();
                        }
                    }

                    if (ally.IsFacing(enemy))
                    {
                        foreach (
                            var itemOnThisAlly in
                                Settings.MCAllyList.Where(
                                    x =>
                                        x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                        ally.HealthPercent <= Settings.AllyHpPercentageCC &&
                                        ally.IsInRange(Player.Instance, 750))
                                    .Where(itemOnThisAlly => mikael.IsOwned() && mikael.IsReady() && HasDebuff(ally)))
                            mikael.Cast(ally);

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage &&
                            ally.IsInRange(Player.Instance, 600))
                        {
                            foreach (
                                var itemOnThisAlly in
                                    Settings.ISAllyList.Where(
                                        x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                        .Where(itemOnThisAlly => ironSolari.IsOwned() && ironSolari.IsReady()))
                                ironSolari.Cast();

                            foreach (
                                var itemOnThisAlly in
                                    Settings.FOTMAllyList.Where(
                                        x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                        .Where(itemOnThisAlly => mountain.IsOwned() && mountain.IsReady()))
                                mountain.Cast(ally);
                        }

                        if (!(enemy.HealthPercent <= Settings.AllyHpPercentageDamage) ||
                            !enemy.IsInRange(Player.Instance, 2200)) continue;

                        foreach (var itemOnThisAlly in Settings.TOAAllyList.Where(
                            x =>
                                x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                ally.IsInRange(Player.Instance, 600))
                            .Where(itemOnThisAlly => talisman.IsOwned() && talisman.IsReady()))
                            talisman.Cast();

                        foreach (var itemOnThisAlly in Settings.FQCAllyList.Where(
                            x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                            .Where(itemOnThisAlly => frostQueen.IsOwned() && frostQueen.IsReady()))
                            frostQueen.Cast(enemy);
                    }
                    else
                    {
                        foreach (
                            var itemOnThisAlly in
                                Settings.MCAllyList.Where(
                                    x =>
                                        x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                        ally.HealthPercent <= Settings.AllyHpPercentageCC &&
                                        ally.IsInRange(Player.Instance, 750))
                                    .Where(itemOnThisAlly => mikael.IsOwned() && mikael.IsReady() && HasDebuff(ally)))
                            mikael.Cast(ally);

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage &&
                            ally.IsInRange(Player.Instance, 600))
                        {
                            foreach (
                                var itemOnThisAlly in
                                    Settings.ISAllyList.Where(
                                        x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                        .Where(itemOnThisAlly => ironSolari.IsOwned() && ironSolari.IsReady()))
                                ironSolari.Cast();

                            foreach (
                                var itemOnThisAlly in
                                    Settings.FOTMAllyList.Where(
                                        x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                        .Where(itemOnThisAlly => mountain.IsOwned() && mountain.IsReady()))
                                mountain.Cast(ally);
                        }

                        if (!(ally.HealthPercent <= Settings.AllyHpPercentageDamage) ||
                            !enemy.IsInRange(Player.Instance, 1650)) continue;

                        foreach (var itemOnThisAlly in Settings.TOAAllyList.Where(
                            x =>
                                x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue &&
                                ally.IsInRange(Player.Instance, 600))
                            .Where(itemOnThisAlly => talisman.IsOwned() && talisman.IsReady()))
                            talisman.Cast();

                        foreach (var itemOnThisAlly in Settings.FQCAllyList.Where(
                            x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                            .Where(itemOnThisAlly => frostQueen.IsOwned() && frostQueen.IsReady()))
                            frostQueen.Cast(enemy);
                    }
                }
            }
        }
    }
}
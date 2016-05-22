using EloBuddy;
using EloBuddy.SDK;
using Settings = PartyJanna.Config.Settings.Items;
using AutoShield = PartyJanna.Config.Settings.AutoShield;
using Humanizer = PartyJanna.Config.Settings.Humanizer;
using System.Linq;

namespace PartyJanna.Modes
{
    public class PermaActive : ModeBase
    {
        public Item ironSolari { get; private set; }
        public Item mountain { get; private set; }
        public Item mikael { get; private set; }
        public Item frostQueen { get; private set; }
        public Item talisman { get; private set; }

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
            if (!Settings.UseItems || Player.Instance.CountEnemiesInRange(2200) == 0)
            { return; }

            ironSolari = new Item(3190, 600);
            mountain = new Item(3401, 600);
            mikael = new Item(3222, 750);
            frostQueen = new Item(3092, 4500);
            talisman = new Item(3069, 600);

            foreach (var enemy in EntityManager.Heroes.Enemies)
            {
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (R.IsLearned && R.IsReady() && AutoShield.AutoUltimate)
                    {
                        foreach (var ultThisAlly in AutoShield.UltAllyList.Where(a => a.DisplayName.Contains(ally.ChampionName) && Player.Instance.IsInRange(ally, R.Range) && a.CurrentValue))
                        {
                            foreach (var slider in AutoShield.UltSliders)
                            {
                                if (ally.HealthPercent <= slider.CurrentValue)
                                {
                                    var js = new System.Action(JannaStop);

                                    R.Cast();

                                    if ()
                                    Core.RepeatAction(js, 250, 1000);
                                }
                            }
                        }
                    }

                    if (ally.IsFacing(enemy))
                    {
                        if (ally.HealthPercent <= Settings.AllyHpPercentageCC && ally.IsInRange(Player.Instance, 750))
                        {
                            if (mikael.IsOwned() && mikael.IsReady() && (ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Poison) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Slow) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt)))
                            {
                                mikael.Cast(ally);
                            }
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage && ally.IsInRange(Player.Instance, 600))
                        {
                            if (ironSolari.IsReady())
                            {
                                ironSolari.Cast();
                            }

                            if (mountain.IsOwned() && mountain.IsReady())
                            {
                                mountain.Cast(ally);
                            }
                        }

                        if (enemy.HealthPercent <= Settings.AllyHpPercentageDamage && enemy.IsInRange(Player.Instance, 2200))
                        {
                            if (talisman.IsOwned() && talisman.IsReady() && ally.IsInRange(Player.Instance, 600))
                            {
                                talisman.Cast();
                            }

                            if (frostQueen.IsOwned() && frostQueen.IsReady())
                            {
                                frostQueen.Cast(enemy);
                            }
                        }
                    }
                    else
                    {
                        if (ally.HealthPercent <= Settings.AllyHpPercentageCC && ally.IsInRange(Player.Instance, 750))
                        {
                            if (mikael.IsOwned() && mikael.IsReady() && (ally.HasBuffOfType(BuffType.Charm) || ally.HasBuffOfType(BuffType.Fear) || ally.HasBuffOfType(BuffType.Poison) || ally.HasBuffOfType(BuffType.Polymorph) || ally.HasBuffOfType(BuffType.Silence) || ally.HasBuffOfType(BuffType.Sleep) || ally.HasBuffOfType(BuffType.Slow) || ally.HasBuffOfType(BuffType.Snare) || ally.HasBuffOfType(BuffType.Stun) || ally.HasBuffOfType(BuffType.Taunt)))
                            {
                                mikael.Cast(ally);
                            }
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage && ally.IsInRange(Player.Instance, 600))
                        {
                            if (ironSolari.IsReady())
                            {
                                ironSolari.Cast();
                            }

                            if (mountain.IsOwned() && mountain.IsReady())
                            {
                                mountain.Cast(ally);
                            }
                        }

                        if (ally.HealthPercent <= Settings.AllyHpPercentageDamage && enemy.IsInRange(Player.Instance, 1650))
                        {
                            if (talisman.IsOwned() && talisman.IsReady() && ally.IsInRange(Player.Instance, 600))
                            {
                                talisman.Cast();
                            }

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

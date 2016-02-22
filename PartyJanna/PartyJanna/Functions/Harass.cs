using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna.Functions
{
    public static class Harass
    {
        private static bool IgnoreMinionCollision { get; set; }

        private static AIHeroClient GetTarget { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Harass";

            GetTarget = TargetSelector.GetTarget(EntityManager.Heroes.Enemies, DamageType.True);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && GetTarget.IsValid && GetTarget.IsEnemy)
            {
                if (Config.Harass.UseQ.CurrentValue && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.Q.Range))
                {
                    if (Player.Instance.CountEnemiesInRange(Config.Spells.Q.Range + 525) <= 2)
                    {
                        IgnoreMinionCollision = false;
                    }
                    else
                    {
                        IgnoreMinionCollision = true;
                    }

                    Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed), IgnoreMinionCollision).CastPosition);

                    if (Config.Harass.UseE.CurrentValue && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
                    {
                        foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
                        {
                            if (Enemy.Spellbook.SpellWasCast && Player.Instance.IsInRange(Enemy, Enemy.CastRange))
                            {
                                Config.Spells.E.Cast(Player.Instance);
                            }
                        }
                    }

                }

                if (Config.Harass.UseW.CurrentValue && Player.Instance.Mana >= Config.Spells.manaW[Config.Spells.W.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(GetTarget);

                    if (Config.Harass.UseE.CurrentValue && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
                    {
                        foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
                        {
                            if (Enemy.Spellbook.SpellWasCast && Player.Instance.IsInRange(Enemy, Enemy.CastRange))
                            {
                                Config.Spells.E.Cast(Player.Instance);
                            }
                        }
                    }
                }
            }
        }
    }
}


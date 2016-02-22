using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna.Functions
{
    public static class Harass
    {
        private static bool IgnoreMinionCollision { get; set; }

        private static AIHeroClient GetTarget { get; set; }

        private static Prediction.Position.PredictionData PredictionData { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Harass";

            GetTarget = TargetSelector.GetTarget(EntityManager.Heroes.Enemies, DamageType.True);

            PredictionData = new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed, Config.Harass.TryToHitMultipleEnemies.CurrentValue ? 2 : 0);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && GetTarget.IsValid && GetTarget.IsEnemy)
            {
                if (Config.Harass.UseQ.CurrentValue && Config.Spells.Q.IsReady() && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.Q.Range))
                {
                    if (Config.Harass.IgnoreCollision.CurrentValue && Player.Instance.CountEnemiesInRange(2200) >= Config.Harass.IgnoreCollisionEnemies.CurrentValue)
                    {
                        Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, PredictionData, true).CastPosition);
                    }
                    else
                    {
                        Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, PredictionData).CastPosition);
                    }

                    if (Config.Harass.UseE.CurrentValue && Config.Spells.E.IsReady() && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
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

                if (Config.Harass.UseW.CurrentValue && Config.Spells.W.IsReady() && Player.Instance.Mana >= Config.Spells.manaW[Config.Spells.W.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(GetTarget);

                    if (Config.Harass.UseE.CurrentValue && Config.Spells.E.IsReady() && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
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


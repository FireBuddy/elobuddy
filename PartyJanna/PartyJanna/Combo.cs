using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;

namespace PartyJanna
{
    public static class Combo
    {
        private static List<AIHeroClient> PriorAllyOrder { get; set; }
        private static List<AIHeroClient> HpAllyOrder { get; set; }

        private static int HighestPriority { get; set; }

        private static float LowestHP { get; set; }

        private static Prediction.Position.PredictionData PredictionData { get; set; }

        private static bool IgnoreMinionCollision { get; set; }

        private static AIHeroClient GetTarget { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Combo";

            PredictionData = new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed, Config.Combo.TryToHitMultipleEnemies.CurrentValue ? 2 : 0);

            HighestPriority = 0;

            LowestHP = int.MaxValue;

            GetTarget = TargetSelector.GetTarget(EntityManager.Heroes.Enemies, DamageType.True);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (GetTarget.IsValid && GetTarget.IsEnemy && Config.Combo.UseQ.CurrentValue && Config.Spells.Q.IsReady() && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.Q.Range))
                {
                    if (Config.Combo.IgnoreCollision.CurrentValue && Player.Instance.CountEnemiesInRange(2200) >= Config.Combo.IgnoreCollisionEnemies.CurrentValue)
                    {
                        Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, PredictionData, true).CastPosition);
                    }
                    else
                    {
                        Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, PredictionData).CastPosition);
                    }
                }

                if (GetTarget.IsValid && GetTarget.IsEnemy && Config.Combo.UseW.CurrentValue && Config.Spells.W.IsReady() && Player.Instance.Mana >= Config.Spells.manaW[Config.Spells.W.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(GetTarget);
                }
            }
        }
    }
}


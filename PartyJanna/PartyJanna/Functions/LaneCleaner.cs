using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna.Functions
{
    public static class LaneCleaner
    {
        private static Prediction.Position.PredictionData PredictionData { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "LaneCleaner";

            PredictionData = new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Config.LaneCleaner.UseQ.CurrentValue && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level])
                {
                    foreach (Obj_AI_Minion EnemyMinion in EntityManager.MinionsAndMonsters.GetLaneMinions())
                    {
                        if (EnemyMinion.IsInRange(Player.Instance, Config.Spells.Q.Range))
                        {
                            Config.Spells.Q.Cast(Prediction.Position.GetPrediction(EnemyMinion, PredictionData, true).CastPosition);
                        }
                    }
                }
            }
        }
    }
}

using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna.Functions
{
    public static class Flee
    {
        private static Prediction.Position.PredictionData PredictionData { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Flee";

            PredictionData = new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                if (Config.Flee.UseQ.CurrentValue && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level])
                {
                    Config.Spells.Q.Cast(Prediction.Position.GetPrediction(TargetSelector.SelectedTarget, PredictionData, true).CastPosition);
                }
            }

            if (Config.Flee.UseE.CurrentValue && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
            {
                foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
                {
                    if (Player.Instance.IsInAutoAttackRange(Enemy))
                    {
                        Config.Spells.E.Cast(Player.Instance);
                    }

                    if (Enemy.Spellbook.SpellWasCast && Player.Instance.IsInRange(Enemy, Enemy.CastRange))
                    {
                        Config.Spells.E.Cast(Player.Instance);

                        if (Config.Flee.UseR.CurrentValue)
                        {
                            Config.Spells.R.Cast();
                        }
                    }
                }
            }

            if (Config.Flee.UseW.CurrentValue && Player.Instance.Mana >= Config.Spells.manaW[Config.Spells.W.Level])
            {
                if (TargetSelector.SelectedTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(TargetSelector.SelectedTarget);
                }
            }
        }
    }
}

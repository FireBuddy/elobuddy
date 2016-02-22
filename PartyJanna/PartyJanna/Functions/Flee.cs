using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna.Functions
{
    public static class Flee
    {
        private static Prediction.Position.PredictionData PredictionData { get; set; }

        private static AIHeroClient GetTarget { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Flee";

            PredictionData = new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed);

            GetTarget = TargetSelector.GetTarget(Config.Spells.Q.Range, DamageType.Mixed);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {

                if (GetTarget.IsValid && GetTarget.IsEnemy && Config.Flee.UseQ.CurrentValue && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.Q.Range))
                {
                    Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, PredictionData, true).CastPosition);
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
                        }
                    }
                }

                if (GetTarget.IsValid && GetTarget.IsEnemy && Config.Flee.UseW.CurrentValue && Player.Instance.Mana >= Config.Spells.manaW[Config.Spells.W.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(GetTarget);
                }
            }
        }
    }
}

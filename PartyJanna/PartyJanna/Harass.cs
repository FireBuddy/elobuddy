using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna
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

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Player.Instance.Mana < Config.Spells.manaW[Config.Spells.W.Level])
                { return; }

                if (Config.Combo.UseQ.CurrentValue && Config.Spells.Q.IsReady() && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.Q.Range))
                {
                    Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Linear, Config.Harass.UseRange.CurrentValue, Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed, Config.Combo.TryToHitMultipleEnemies.CurrentValue ? 2 : 0), true).CastPosition);
                }

                if (Config.Combo.UseW.CurrentValue && Config.Spells.W.IsReady() && GetTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(GetTarget);
                }
            }
        }
    }
}


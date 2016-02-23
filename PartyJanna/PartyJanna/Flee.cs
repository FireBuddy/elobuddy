using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna
{
    public static class Flee
    {
        private static AIHeroClient GetTarget { get; set; }

        public static void Execute()
        {
            Startup.CurrentFunction = "Flee";

            GetTarget = TargetSelector.GetTarget(EntityManager.Heroes.Enemies, DamageType.True);

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {

                if (GetTarget.IsValid && GetTarget.IsEnemy && Config.Flee.UseQ.CurrentValue && Config.Spells.Q.IsReady() && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.Q.Range))
                {
                    Config.Spells.Q.Cast(Prediction.Position.GetPrediction(GetTarget, new Prediction.Position.PredictionData(Prediction.Position.PredictionData.PredictionType.Circular, Convert.ToInt32(Config.Spells.Q.Range), Config.Spells.Q.Width, Config.Spells.Q.ConeAngleDegrees, Config.Spells.Q.CastDelay, Config.Spells.Q.Speed), true).CastPosition);
                }

                if (Config.Flee.UseE.CurrentValue && Config.Spells.E.IsReady() && Player.Instance.Mana >= Config.Spells.manaE[Config.Spells.E.Level])
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

                if (GetTarget.IsValid && GetTarget.IsEnemy && Config.Flee.UseW.CurrentValue && Config.Spells.W.IsReady() && Player.Instance.Mana >= Config.Spells.manaW[Config.Spells.W.Level] && GetTarget.IsInRange(Player.Instance, Config.Spells.W.Range))
                {
                    Config.Spells.W.Cast(GetTarget);
                }
            }
        }
    }
}

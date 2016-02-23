using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyJanna
{
    public static class LaneCleaner
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "LaneCleaner";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Config.LaneCleaner.UseQ.CurrentValue && Player.Instance.Mana >= Config.Spells.manaQ[Config.Spells.Q.Level])
            {
                foreach (Obj_AI_Minion EnemyMinion in EntityManager.MinionsAndMonsters.GetLaneMinions())
                {
                    if (EnemyMinion.IsInRange(Player.Instance, Config.Spells.Q.Range))
                    {
                        Config.Spells.Q.Cast(EnemyMinion);
                    }
                }
            }
        }
    }
}

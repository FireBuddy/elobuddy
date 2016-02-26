using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            foreach (Obj_AI_Minion EnemyMinion in EntityManager.MinionsAndMonsters.GetLaneMinions())
            {
                if (EnemyMinion.IsInRange(Player.Instance, Q.Range))
                {
                    Q.Cast(EnemyMinion);
                }
            }
        }
    }
}

using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted() => Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);

        public override void Execute()
        {
            foreach (
                var enemyMinion in
                    EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .Where(enemyMinion => enemyMinion.IsInRange(Player.Instance, Q.Range)))
                Q.Cast(enemyMinion);
        }
    }
}
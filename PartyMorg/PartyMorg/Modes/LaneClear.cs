using EloBuddy;
using EloBuddy.SDK;
using System;

namespace PartyMorg.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var farmLocation = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(EntityManager.MinionsAndMonsters.GetLaneMinions(), 500, Convert.ToInt32(W.Range));

            if (farmLocation.HitNumber >= 3)
            {
                W.Cast(farmLocation.CastPosition);
            }
        }
    }
}

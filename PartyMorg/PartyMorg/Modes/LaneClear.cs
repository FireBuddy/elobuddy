using EloBuddy;
using EloBuddy.SDK;
using System;
using Settings = PartyMorg.Config.Settings.LaneClear;

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
            var farmLocation = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(EntityManager.MinionsAndMonsters.GetLaneMinions(), 300, Convert.ToInt32(W.Range));

            if (farmLocation.HitNumber >= Settings.MinionsToUseW && Settings.UseW)
            {
                W.Cast(farmLocation.CastPosition);
            }
        }
    }
}

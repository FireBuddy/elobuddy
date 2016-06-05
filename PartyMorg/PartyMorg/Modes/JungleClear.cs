using System;
using EloBuddy.SDK;
using Settings = PartyMorg.Config.Settings.JungleClear;

namespace PartyMorg.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted() => Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);

        public override void Execute()
        {
            var farmLocation =
                EntityManager.MinionsAndMonsters.GetLineFarmLocation(
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(), 80, Convert.ToInt32(Q.Range));

            if (Settings.UseQ)
                Q.Cast(farmLocation.CastPosition);

            if (!Settings.UseW) return;

            farmLocation =
                EntityManager.MinionsAndMonsters.GetCircularFarmLocation(
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(), 300, Convert.ToInt32(W.Range));

            W.Cast(farmLocation.CastPosition);
        }
    }
}
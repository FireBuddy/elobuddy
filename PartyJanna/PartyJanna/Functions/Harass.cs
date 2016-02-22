using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Functions
{
    public static class Harass
    {
        public static void Execute()
        {
            Startup.CurrentFunction = "Harass";

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Config.MyHero.Mana >= Config.Spells.manaW[Config.Spells.W.Level])
                {
                    foreach (AIHeroClient Enemy in EntityManager.Heroes.Enemies)
                    {
                        if (Enemy.IsInRange(Config.MyHero, Config.Spells.W.Range))
                        {

                        }
                    }
                }
            }
        }
    }
}

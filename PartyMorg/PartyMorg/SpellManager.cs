using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace PartyMorg
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1300, SkillShotType.Linear, 250, 1200, 80);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 250, null, 300);
            E = new Spell.Targeted(SpellSlot.E, 750);
            R = new Spell.Active(SpellSlot.R, 625);
        }

        public static void Initialize()
        {
        }
    }
}
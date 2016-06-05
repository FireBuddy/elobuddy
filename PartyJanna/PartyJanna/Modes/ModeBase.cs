using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Skillshot Q => SpellManager.Q;

        protected static Spell.Targeted W => SpellManager.W;

/*
        protected Spell.Targeted E => SpellManager.E;
*/

        protected static Spell.Active R => SpellManager.R;

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();

        protected static AIHeroClient GetTarget(Spell.SpellBase spell, DamageType damageType)
        {
            return TargetSelector.SelectedTarget != null && spell.IsInRange(TargetSelector.SelectedTarget)
                ? TargetSelector.SelectedTarget
                : TargetSelector.GetTarget(spell.Range, damageType, Player.Instance.Position);
        }

        protected static bool IsImmobile(Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Stun) ||
                   target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Snare) ||
                   target.HasBuffOfType(BuffType.Taunt) || target.HasBuffOfType(BuffType.Suppression);
        }

        protected static bool HasDebuff(Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Fear) ||
                   target.HasBuffOfType(BuffType.Poison) || target.HasBuffOfType(BuffType.Polymorph) ||
                   target.HasBuffOfType(BuffType.Silence) || target.HasBuffOfType(BuffType.Sleep) ||
                   target.HasBuffOfType(BuffType.Slow) || target.HasBuffOfType(BuffType.Snare) ||
                   target.HasBuffOfType(BuffType.Stun) ||
                   target.HasBuffOfType(BuffType.Taunt);
        }
    }
}
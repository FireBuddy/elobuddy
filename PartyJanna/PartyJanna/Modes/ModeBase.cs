using EloBuddy;
using EloBuddy.SDK;

namespace PartyJanna.Modes
{
    public abstract class ModeBase
    {
        protected Spell.Skillshot Q
        {
            get { return SpellManager.Q; }
        }
        protected Spell.Targeted W
        {
            get { return SpellManager.W; }
        }
        protected Spell.Targeted E
        {
            get { return SpellManager.E; }
        }
        protected Spell.Active R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();

        public static AIHeroClient GetTarget(Spell.SpellBase spell, DamageType damageType)
        {
            return TargetSelector.SelectedTarget != null && spell.IsInRange(TargetSelector.SelectedTarget) ? TargetSelector.SelectedTarget : TargetSelector.GetTarget(spell.Range, damageType, Player.Instance.Position) != null ? TargetSelector.GetTarget(spell.Range, damageType, Player.Instance.Position) : null;
        }

        public static bool Immobile(Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Taunt) || target.HasBuffOfType(BuffType.Suppression);
        }
    }
}

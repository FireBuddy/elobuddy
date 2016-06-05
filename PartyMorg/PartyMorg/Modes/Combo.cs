using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using static PartyMorg.Config.Settings;
using Settings = PartyMorg.Config.Settings.Combo;

namespace PartyMorg.Modes
{
    public sealed class Combo : ModeBase
    {
        private static readonly Item zhonyasHourglass = new Item(3157);

        private static Spell.Skillshot flashSpell { get; set; }

        public override bool ShouldBeExecuted() => Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);

        public override void Execute()
        {
            flashSpell = new Spell.Skillshot(Player.Instance.GetSpellSlotFromName("summonerflash"), 425,
                SkillShotType.Linear);

            var target = GetTarget(Q, DamageType.Magical);

            PredictionResult pred;

            if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) && Settings.UseQ &&
                !target.IsDead)
            {
                pred = Q.GetPrediction(target);

                if (Humanizer.QCastDelayEnabled)
                {
                    if (pred.HitChancePercent >= Settings.QMinHitChance)
                        Core.DelayAction(() => { Q.Cast(pred.CastPosition); },
                            Humanizer.QRndmDelay
                                ? new Random().Next(250, Humanizer.QCastDelay)
                                : Humanizer.QCastDelay);
                }
                else
                {
                    if (pred.HitChancePercent >= Settings.QMinHitChance)
                        Q.Cast(pred.CastPosition);
                }
            }

            if (Settings.WImmobileOnly)
            {
                if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) &&
                    Settings.UseW && !target.IsDead && Player.Instance.IsInRange(target, W.Range) && IsImmobile(target))
                {
                    pred = W.GetPrediction(target);
                    W.Cast(pred.CastPosition);
                }
            }
            else
            {
                if (target != null && target.IsTargetable && !target.HasBuffOfType(BuffType.SpellImmunity) &&
                    Settings.UseW && !target.IsDead && Player.Instance.IsInRange(target, W.Range))
                {
                    pred = W.GetPrediction(target);

                    if (Settings.UseQBeforeW)
                    {
                        if (Q.IsOnCooldown)
                        {
                            W.Cast(pred.CastPosition);
                        }
                    }
                    else
                    {
                        W.Cast(pred.CastPosition);
                    }
                }
            }

            target = GetTarget(R, DamageType.Magical);

            if (!R.IsReady() || !Settings.UseR || target == null || !target.IsTargetable ||
                target.HasBuffOfType(BuffType.SpellImmunity) || target.IsDead) return;

            if (Player.Instance.CountEnemiesInRange(Settings.UltMinRange) == 0 && Settings.FlashUlt)
            {
                var enemiesFaced = 0;

                foreach (var enemy in EntityManager.Heroes.Enemies)
                {
                    if (Player.Instance.IsFacing(enemy))
                        enemiesFaced++;

                    if (enemiesFaced < Settings.RMinEnemies ||
                        Player.Instance.CountEnemiesInRange(Settings.UltMinRange + flashSpell.Range) <
                        Settings.RMinEnemies) continue;

                    flashSpell.Cast(Player.Instance.Position.Extend(enemy.Position, flashSpell.Range).To3D());

                    R.Cast();

                    if (Settings.UltZhonya)
                        zhonyasHourglass.Cast();

                    enemiesFaced = 0;
                }
            }
            else
            {
                if (!target.IsTargetable || target.HasBuffOfType(BuffType.SpellImmunity) || !Settings.UseR ||
                    Player.Instance.CountEnemiesInRange(Settings.UltMinRange) < Settings.RMinEnemies || target.IsDead)
                    return;

                if (Humanizer.RCastDelayEnabled)
                {
                    if (Humanizer.RRndmDelay)
                    {
                        Core.DelayAction(() => { R.Cast(); }, new Random().Next(250, Humanizer.RCastDelay));

                        if (Settings.UltZhonya)
                            zhonyasHourglass.Cast();
                    }
                    else
                    {
                        Core.DelayAction(() => { R.Cast(); }, Humanizer.RCastDelay);

                        if (Settings.UltZhonya)
                            zhonyasHourglass.Cast();
                    }
                }
                else
                {
                    R.Cast();

                    if (Settings.UltZhonya)
                        zhonyasHourglass.Cast();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using static PartyJanna.SpellManager;
using _Interrupter = PartyJanna.Config.Settings.Interrupter;

namespace PartyJanna
{
    public static class Events
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();

        static Events()
        {
            Obj_AI_Base.OnBasicAttack += OnBasicAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Gapcloser.OnGapcloser += OnGapcloser;
            Interrupter.OnInterruptableSpell += OnInterruptableSpell;
        }

        private static List<AIHeroClient> priorAllyOrder { get; set; }
        private static List<AIHeroClient> hpAllyOrder { get; set; }
        private static int highestPriority { get; set; }
        private static float lowestHP { get; set; }

        public static void Initialize()
        {
        }

        private static void CastShield(Obj_AI_Base target)
        {
            if (Config.Settings.Humanizer.ECastDelayEnabled)
                Core.DelayAction(() => { E.Cast(target); },
                    Config.Settings.Humanizer.ERndmDelay
                        ? new Random().Next(250, Config.Settings.Humanizer.ECastDelay)
                        : Config.Settings.Humanizer.ECastDelay);
            else
                E.Cast(target);
        }

        private static void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!sender.IsEnemy || !Config.Settings.AntiGapcloser.AntiGap || Player.Instance.IsRecalling()) return;

            foreach (
                var ally in
                    EntityManager.Heroes.Allies.Where(ally => sender.IsFacing(ally) && Q.IsInRange(sender.Position)))
            {
                if (Config.Settings.Humanizer.QCastDelayEnabled)
                {
                    if (Config.Settings.Humanizer.QCastDelayEnabled)
                        Core.DelayAction(() => { Q.Cast(Q.GetPrediction(sender).CastPosition); },
                            Config.Settings.Humanizer.QRndmDelay
                                ? new Random().Next(250, Config.Settings.Humanizer.QCastDelay)
                                : Config.Settings.Humanizer.QCastDelay);
                    else
                        Q.Cast(Q.GetPrediction(sender).CastPosition);
                }
                else
                    Q.Cast(Q.GetPrediction(sender).CastPosition);
            }
        }

        private static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy || Player.Instance.IsRecalling())
            {
                return;
            }

            if (e.DangerLevel == DangerLevel.High)
            {
                if (_Interrupter.RInterruptDangerous && R.IsReady() && R.IsInRange(sender) &&
                    Player.Instance.Mana >= 100)
                {
                    if (Config.Settings.Humanizer.RCastDelayEnabled)
                        Core.DelayAction(() => { R.Cast(); },
                            Config.Settings.Humanizer.RRndmDelay
                                ? new Random().Next(250, Config.Settings.Humanizer.RCastDelay)
                                : Config.Settings.Humanizer.RCastDelay);
                    else
                        R.Cast();
                }
                else
                {
                    if (!_Interrupter.QInterruptDangerous || !Q.IsReady() || !Q.IsInRange(sender)) return;

                    if (Config.Settings.Humanizer.QCastDelayEnabled)
                    {
                        if (Config.Settings.Humanizer.QCastDelayEnabled)
                            Core.DelayAction(() => { Q.Cast(Q.GetPrediction(sender).CastPosition); },
                                Config.Settings.Humanizer.QRndmDelay
                                    ? new Random().Next(250, Config.Settings.Humanizer.QCastDelay)
                                    : Config.Settings.Humanizer.QCastDelay);
                        else
                            Q.Cast(Q.GetPrediction(sender).CastPosition);
                    }
                    else
                        Q.Cast(Q.GetPrediction(sender).CastPosition);
                }
            }
            else
            {
                if (!_Interrupter.QInterrupt || !Q.IsReady() || !Q.IsInRange(sender)) return;

                if (Config.Settings.Humanizer.QCastDelayEnabled)
                {
                    if (Config.Settings.Humanizer.QCastDelayEnabled)
                        Core.DelayAction(() => { Q.Cast(Q.GetPrediction(sender).CastPosition); },
                            Config.Settings.Humanizer.QRndmDelay
                                ? new Random().Next(250, Config.Settings.Humanizer.QCastDelay)
                                : Config.Settings.Humanizer.QCastDelay);
                    else
                        Q.Cast(Q.GetPrediction(sender).CastPosition);
                }
                else
                    Q.Cast(Q.GetPrediction(sender).CastPosition);
            }
        }

        private static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsRecalling()) return;

            priorAllyOrder = new List<AIHeroClient>();

            hpAllyOrder = new List<AIHeroClient>();

            highestPriority = 0;

            lowestHP = int.MaxValue;

            if (sender.IsEnemy && sender.IsMinion)
            {
                foreach (
                    var ally in
                        from ally in EntityManager.Heroes.Allies.Where(ally => ally.CountEnemiesInRange(1000) == 0)
                        from shieldThisAlly in
                            Config.Settings.AutoShield.ShieldAllyList.Where(
                                x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                        where args.Target == ally
                        select ally)
                    CastShield(sender);

                if (Config.Settings.AutoShield.TurretShieldMinion)
                {
                    foreach (
                        var turret in
                            EntityManager.Turrets.Allies.Where(
                                turret => args.Target == turret && Player.Instance.IsInRange(turret, E.Range)))
                        CastShield(turret);
                }
            }

            if (sender.IsAlly && sender.IsRanged && !sender.IsMinion && Config.Settings.AutoShield.BoostAD)
            {
                foreach (var enemy in from enemy in EntityManager.Heroes.Enemies
                    from shieldThisAlly in
                        Config.Settings.AutoShield.ShieldAllyList.Where(
                            x => x.DisplayName.Contains(sender.Name) && x.CurrentValue)
                    where args.Target == enemy
                    select enemy)
                    CastShield(sender);
            }

            if (sender.IsEnemy && !sender.IsMinion)
            {
                if (Config.Settings.AutoShield.PriorMode == 0)
                {
                    foreach (var ally in EntityManager.Heroes.Allies)
                        if (ally.Health <= lowestHP)
                        {
                            lowestHP = ally.Health;
                            hpAllyOrder.Insert(0, ally);
                        }
                        else
                            hpAllyOrder.Add(ally);

                    foreach (
                        var ally in from ally in hpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, E.Range))
                            from shieldThisAlly in
                                Config.Settings.AutoShield.ShieldAllyList.Where(
                                    x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                            where args.Target == ally
                            select ally)
                        CastShield(ally);
                }
                else
                {
                    foreach (var slider in Config.Settings.AutoShield.Sliders)
                        if (slider.CurrentValue >= highestPriority)
                        {
                            highestPriority = slider.CurrentValue;

                            foreach (
                                var ally in
                                    Config.Settings.AutoShield.Heros.Where(
                                        ally => slider.VisibleName.Contains(ally.ChampionName)))
                                priorAllyOrder.Insert(0, ally);
                        }
                        else
                        {
                            foreach (
                                var ally in
                                    Config.Settings.AutoShield.Heros.Where(
                                        ally => slider.VisibleName.Contains(ally.ChampionName)))
                                priorAllyOrder.Add(ally);
                        }

                    foreach (
                        var ally in from ally in priorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, E.Range))
                            from shieldThisAlly in
                                Config.Settings.AutoShield.ShieldAllyList.Where(
                                    x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                            where args.Target == ally
                            select ally)
                        CastShield(ally);
                }

                if (Config.Settings.AutoShield.TurretShieldChampion)
                    foreach (
                        var turret in
                            EntityManager.Turrets.Allies.Where(
                                turret => args.Target == turret && Player.Instance.IsInRange(turret, E.Range)))
                        CastShield(turret);
            }

            if (!Config.Settings.AutoShield.SelfShield) return;

            if (args.Target != null && args.Target.IsMe)
                CastShield(Player.Instance);
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.Instance.IsRecalling()) return;

            foreach (
                var enemy in
                    EntityManager.Heroes.Enemies.Where(
                        enemy => sender.IsAlly && Player.Instance.IsInRange(sender, E.Range) &&
                                 args.SData.Name == "EzrealMysticShotMissile" && sender.CountEnemiesInRange(1200) > 0 &&
                                 Prediction.Position.PredictUnitPosition(enemy, 250).IsInRange(args.End, 60)))
                CastShield(sender);

            if (!sender.IsEnemy) return;

            priorAllyOrder = new List<AIHeroClient>();

            hpAllyOrder = new List<AIHeroClient>();

            highestPriority = 0;

            lowestHP = int.MaxValue;

            if (Config.Settings.AutoShield.PriorMode == 1)
            {
                foreach (var slider in Config.Settings.AutoShield.Sliders)
                {
                    if (slider.CurrentValue >= highestPriority)
                    {
                        highestPriority = slider.CurrentValue;

                        foreach (
                            var ally in
                                Config.Settings.AutoShield.Heros.Where(
                                    ally => slider.VisibleName.Contains(ally.ChampionName)))
                            priorAllyOrder.Insert(0, ally);
                    }
                    else
                    {
                        foreach (
                            var ally in
                                Config.Settings.AutoShield.Heros.Where(
                                    ally => slider.VisibleName.Contains(ally.ChampionName)))
                            priorAllyOrder.Add(ally);
                    }
                }

                foreach (var ally in priorAllyOrder.Where(ally => Player.Instance.IsInRange(ally, E.Range)))
                {
                    foreach (
                        var shieldThisSpell in
                            from shieldThisAlly in
                                Config.Settings.AutoShield.ShieldAllyList.Where(
                                    x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                            from shieldThisSpell in Config.Settings.AutoShield.ShieldSpellList
                            where shieldThisSpell.DisplayName.Contains(args.SData.Name) && shieldThisSpell.CurrentValue
                            select shieldThisSpell)
                    {
                        if (args.Target == ally)
                            CastShield(ally);
                        else
                        {
                            if (Prediction.Position.PredictUnitPosition(ally, 250)
                                .IsInRange(args.End,
                                    MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 1]))
                            {
                                CastShield(ally);
                            }
                            else if (sender.IsFacing(ally) &&
                                     Prediction.Position.PredictUnitPosition(ally, 250)
                                         .IsInRange(sender,
                                             MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 0]))
                            {
                                CastShield(ally);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var ally in EntityManager.Heroes.Allies)
                {
                    if (ally.Health <= lowestHP)
                    {
                        lowestHP = ally.Health;
                        hpAllyOrder.Insert(0, ally);
                    }
                    else
                        hpAllyOrder.Add(ally);
                }

                foreach (var ally in hpAllyOrder.Where(ally => Player.Instance.IsInRange(ally, E.Range)))
                {
                    foreach (
                        var shieldThisSpell in
                            Config.Settings.AutoShield.ShieldAllyList.Where(
                                x => x.DisplayName.Contains(ally.ChampionName) && x.CurrentValue)
                                .SelectMany(
                                    shieldThisAlly =>
                                        Config.Settings.AutoShield.ShieldSpellList.Where(
                                            s => s.DisplayName.Contains(args.SData.Name) && s.CurrentValue)))
                    {
                        if (args.Target == ally)
                            CastShield(ally);
                        else
                        {
                            if (Prediction.Position.PredictUnitPosition(ally, 250)
                                .IsInRange(args.End,
                                    MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 1]))
                            {
                                CastShield(ally);
                            }
                            else if (sender.IsFacing(ally) &&
                                     Prediction.Position.PredictUnitPosition(ally, 250)
                                         .IsInRange(sender,
                                             MissileDatabase.rangeRadiusDatabase[shieldThisSpell.DisplayName.Last(), 0]))
                            {
                                CastShield(ally);
                            }
                        }
                    }
                }
            }
        }
    }
}
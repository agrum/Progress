﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Asset.Skill
{
    public class LaserBeam
    {
        public Data.Skill.Skill Dta { get; private set; }

        public LaserBeam()
        {
            //NAMES
            Data.NamedHash name = new Data.NamedHash(GetType().ToString());
            string description = "Fire a wave that damages all enemies in its path.";
            string details = "Throw a projectile that deals #damage# damage to all enemy untis in its path.";
            string Damage = "Damage";
            string Cooldown = "Cooldown";
            string Range = "Range";
            string Width = "Width";
            string CastTime = "CastTime";

            //METRICS
            List<Data.Skill.Metric> metrics = new List<Data.Skill.Metric>();
            metrics.Add(new Data.Skill.Metric(
                Damage,
                Data.Skill.Metric.ECategory.Misc,
                new Data.Skill.NumericBuilder()
                .OpenP()
                .Value(-400)
                .Mul()
                .Stat(Data.Skill.ESubject.Source, Data.Skill.Unit.Stat.EStandard.Power.ToString())
                .CloseP()
                .Add()
                .OpenP()
                .Value(-200)
                .Mul()
                .Gauge(Data.Skill.ESubject.Target, Data.Skill.Unit.Gauge.EStandard.Health.ToString(), Data.Skill.Unit.Gauge.EExtract.MissingRatio)
                .CloseP()
                .Validate(),
                new Data.Skill.Metric.UpgradeType(
                    Data.Skill.Metric.UpgradeType.ESign.Positive,
                    6,
                    1.1)));
            metrics.Add(new Data.Skill.Metric(
                Cooldown,
                Data.Skill.Metric.ECategory.Misc,
                new Data.Skill.NumericBuilder()
                .Value(24)
                .Div()
                .Stat(Data.Skill.ESubject.Source, Data.Skill.Unit.Stat.EStandard.Haste.ToString())
                .Validate(),
                new Data.Skill.Metric.UpgradeType(
                    Data.Skill.Metric.UpgradeType.ESign.Negative,
                    6,
                    1.1)));
            metrics.Add(new Data.Skill.Metric(
                Range,
                Data.Skill.Metric.ECategory.Misc,
                new Data.Skill.NumericBuilder()
                .Value(20)
                .Validate(),
                new Data.Skill.Metric.UpgradeType(
                    Data.Skill.Metric.UpgradeType.ESign.Positive,
                    6,
                    1.1)));
            metrics.Add(new Data.Skill.Metric(
                Width,
                Data.Skill.Metric.ECategory.Misc,
                new Data.Skill.NumericBuilder()
                .Value(2)
                .Validate(),
                new Data.Skill.Metric.UpgradeType(
                    Data.Skill.Metric.UpgradeType.ESign.Positive,
                    6,
                    1.1)));
            metrics.Add(new Data.Skill.Metric(
                CastTime,
                Data.Skill.Metric.ECategory.Misc,
                new Data.Skill.NumericBuilder()
                .Value(1.5)
                .Div()
                .Stat(Data.Skill.ESubject.Source, Data.Skill.Unit.Stat.EStandard.Haste.ToString())
                .Validate(),
                new Data.Skill.Metric.UpgradeType(
                    Data.Skill.Metric.UpgradeType.ESign.Negative,
                    6,
                    1.1)));

            //PASSIVES
            List<Data.Skill.ModifierBehaviour> passives = new List<Data.Skill.ModifierBehaviour>();

            //LAYERS
            List <Data.Skill.Layer.Base> layers = new List<Data.Skill.Layer.Base>();
            layers.Add(new Data.Skill.Layer.Active(
                new Data.Skill.Layer.Visual(Cooldown),
                new Data.Skill.Layer.Activation.Press(),
                new Data.Skill.Layer.Control.Beam(Range, Width),
                new Data.Skill.Condition(Cooldown, Data.Skill.Condition.ERule.EqualTo, 0),
                new Data.Skill.Layer.Medium.Cast(CastTime),
                new Data.Skill.Effect.Area(
                    "BeamEffect",
                    Data.Skill.Effect.Area.EShape.Beam,
                    Range,
                    Width,
                    0,
                    0,
                    new Data.Skill.Effect.Stat(
                        "BeamEffectDamage",
                        Data.Skill.Unit.Gauge.EStandard.Health.ToString(),
                        Data.Skill.Unit.Stat.ECategory.Magical,
                        Damage
                        ))));

            //BUILD
            Dta = new Data.Skill.Skill(
                Data.Skill.Skill.ECategory.Ability,
                name,
                description,
                details,
                metrics,
                passives,
                layers);
        }
    }
}

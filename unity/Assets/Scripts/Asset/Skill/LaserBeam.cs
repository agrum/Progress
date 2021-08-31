using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Asset.Skill
{
    public class LaserBeam
    {
        static public readonly string _Id = "612d99c7e850138fcd5f55d8";
        static public readonly Data.NamedHash Name = new Data.NamedHash("Laser Beam");
        static public readonly string BaseDamageFactor = "BaseDamage";
        static public readonly string HealthDamageFactor = "HealthDamage";
        static public readonly string DamageEquation = "TotalDamage";
        static public readonly string Cooldown = "Cooldown";
        static public readonly string Length = "Length";
        static public readonly string Width = "Width";
        static public readonly string CastTime = "CastTime";

        static public Data.Skill.Skill GetData()
        {
            //NAMES
            string description = "Fire a wave that damages all enemies in its path.";
            string details = "Throw a projectile that deals #BaseDamage# plus #HealthDamage#% of your missing health as elemental damage to all enemy units in its path.";

            //METRICS
            List<Data.Skill.Metric> metrics = new List<Data.Skill.Metric>() {
            new Data.Skill.Metric(
                BaseDamageFactor,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.Elemental, Data.Skill.Metric.ETag.Spell },
                new Data.Skill.NumericBuilder()
                .Value(-400)
                .Validate()),
            new Data.Skill.Metric(
                HealthDamageFactor,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.Elemental, Data.Skill.Metric.ETag.Spell },
                new Data.Skill.NumericBuilder()
                .Value(2)
                .Validate()),
            new Data.Skill.Metric(
                DamageEquation,
                new List<Data.Skill.Metric.ETag> { },
                new Data.Skill.NumericBuilder()
                .OpenP()
                .Value(-1)
                .Mul()
                .Metric(BaseDamageFactor)
                .Mul()
                .Stat(Data.Skill.ESubject.Source, Data.Skill.Unit.Stat.EStandard.Power.ToString())
                .CloseP()
                .Add()
                .OpenP()
                .Value(-100)
                .Mul()
                .Metric(HealthDamageFactor)
                .Mul()
                .Gauge(Data.Skill.ESubject.Target, Data.Skill.Unit.Gauge.EStandard.Health.ToString(), Data.Skill.Unit.Gauge.EExtract.MissingRatio)
                .CloseP()
                .Validate()),
            new Data.Skill.Metric(
                Cooldown,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.Cooldown },
                new Data.Skill.NumericBuilder()
                .Value(24)
                .Div()
                .Stat(Data.Skill.ESubject.Source, Data.Skill.Unit.Stat.EStandard.Haste.ToString())
                .Validate()),
            new Data.Skill.Metric(
                Length,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.AoE },
                new Data.Skill.NumericBuilder()
                .Value(20)
                .Validate()),
            new Data.Skill.Metric(
                Width,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.AoE },
                new Data.Skill.NumericBuilder()
                .Value(2)
                .Validate()),
            new Data.Skill.Metric(
                CastTime,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.ActionSpeed },
                new Data.Skill.NumericBuilder()
                .Value(1.5)
                .Div()
                .Stat(Data.Skill.ESubject.Source, Data.Skill.Unit.Stat.EStandard.Haste.ToString())
                .Validate())
            };

            //BUILD
            return new Data.Skill.Skill(
                Data.Skill.Skill.ECategory.Ability,
                Name,
                description,
                details,
                metrics);
        }

        static public string GetString()
        {
            JSONNode jsonData = GetData();
            return jsonData.ToString();
        }
    }
}

using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Asset.SkillExport
{
    public class Dash
    {
        static public readonly string Range = "Range";
        static public readonly string MaxSpeedMultiplier = "MaxSpeedMultiplier";
        static public readonly string MinSpeedMultiplier = "MinSpeedMultiplier";
        static public readonly string Cooldown = "Cooldown";
        static public readonly string CastTime = "CastTime";

        static public Data.Skill.Skill GetData()
        {
            //NAMES
            Data.NamedHash name = new Data.NamedHash("Dash");
            string description = "Fire a wave that damages all enemies in its path.";
            string details = "Throw a projectile that deals #BaseDamage# plus #HealthDamage#% of your missing health as elemental damage to all enemy units in its path.";

            //METRICS
            List<Data.Skill.Metric> metrics = new List<Data.Skill.Metric>() {
            new Data.Skill.Metric(
                Range,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.Cooldown },
                new Data.Skill.NumericBuilder()
                .Value(5)
                .Validate()),
            new Data.Skill.Metric(
                MaxSpeedMultiplier,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.Cooldown },
                new Data.Skill.NumericBuilder()
                .Value(2)
                .Validate()),
            new Data.Skill.Metric(
                MinSpeedMultiplier,
                new List<Data.Skill.Metric.ETag> { Data.Skill.Metric.ETag.Cooldown },
                new Data.Skill.NumericBuilder()
                .Value(6)
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
                name,
                description,
                details,
                metrics);
        }

        static public string GetString() { 
            JSONNode jsonData = GetData();
            return jsonData.ToString();
        }
    }
}

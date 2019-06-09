using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class SkillUpgrade
    {
        public Data.Skill.Skill Skill { get; private set; } = null;
        private Dictionary<Data.Skill.Metric, MetricUpgrade> metricUpgradeMap = new Dictionary<Data.Skill.Metric, MetricUpgrade>();

        public MetricUpgrade this[Data.Skill.Metric metric_]
        {
            get
            {
                return metricUpgradeMap[metric_];
            }
        }

        public static implicit operator JSONNode(SkillUpgrade object_)
        {
            JSONObject json = new JSONObject();

            json["_id"] = object_.Skill.Name;
            var ugprades = new JSONArray();
            foreach (var upgrade in object_.metricUpgradeMap)
            {
                if (upgrade.Value.Level != 0)
                    ugprades.Add(upgrade.Value);
            }
            json["Upgrades"] = ugprades;

            return json;
        }

        public SkillUpgrade(Data.Skill.Skill skill_)
        {
            Skill = skill_;

            foreach (var metric in Skill.Metrics)
                metricUpgradeMap.Add(metric, new MetricUpgrade(metric, 0));
        }

        public SkillUpgrade(JSONObject json_)
        {            
            var constellation = App.Content.ConstellationList[App.Content.GameSettings.Json["Constellation"]];
            Skill = constellation.Skill(json_["Skill"]);
            System.Func<Data.Skill.Metric, JSONObject> lookUp = (Data.Skill.Metric metric_) =>
            {
                foreach (var node in json_["Upgrades"].AsArray)
                    if (node.Value["_id"] == metric_.Name)
                        return node.Value.AsObject;
                return null;
            };

            foreach (var metric in Skill.Metrics)
                metricUpgradeMap.Add(metric, new MetricUpgrade(metric, lookUp(metric)));
        }

        public Dictionary<Data.Skill.Metric, MetricUpgrade>.ValueCollection MetricUpgradeList()
        {
            return metricUpgradeMap.Values;
        }

        public bool IsValid()
        {
            if (metricUpgradeMap.Count == 0)
                return false;

            foreach (var metricUpgrade in metricUpgradeMap)
            {
                if (metricUpgrade.Value.Level != 0)
                    return true;
            }

            return false;
        }

        public int Level()
        {
            double cumulativeLevel = 0;
            foreach (var metricUpgrades in metricUpgradeMap)
                cumulativeLevel += System.Math.Abs(metricUpgrades.Value.Level);
            return (int) cumulativeLevel;
        }

        public double OverallWeight()
        {
            return Level() / 30.0f;
        }

        public int Handicap()
        {
            double cumulativeLevel = 0;
            foreach (var metricUpgrades in metricUpgradeMap)
                cumulativeLevel += metricUpgrades.Value.Level;
            return (int) cumulativeLevel;
        }

        public static double OverallWeight(double cumulativeLevel_)
        {
            return cumulativeLevel_ / 30.0f;
        }
    }
}

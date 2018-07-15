using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class SkillUpgrade
    {
        public MetricUpgrade this[SkillMetric metric_]
        {
            get
            {
                return metricUpgradeMap[metric_];
            }
        }
        public JSONObject Json
        {
            get
            {
                JSONObject json = new JSONObject();

                json["skill"] = Skill.Uuid;
                json["upgrades"] = new JSONArray();
                foreach (var upgrade in metricUpgradeMap)
                {
                    if (upgrade.Value.Level != 0)
                        json["upgrades"].Add(upgrade.Value.Json);
                }

                return json;
            }
        }

        public Skill Skill { get; private set; } = null;
        private Dictionary<SkillMetric, MetricUpgrade> metricUpgradeMap = new Dictionary<SkillMetric, MetricUpgrade>();

        public SkillUpgrade(Skill skill_)
        {
            Skill = skill_;

            foreach (var metric in Skill.MetrictList)
                metricUpgradeMap.Add(metric, new MetricUpgrade(metric, null));
        }

        public SkillUpgrade(JSONObject json_)
        {            
            var constellation = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
            Skill = constellation.Skill(json_["skill"]);
            System.Func<SkillMetric, JSONObject> lookUp = (SkillMetric metric_) =>
            {
                foreach (var node in json_["upgrades"].AsArray)
                    if (node.Value["metric"] == metric_.Json["_id"])
                        return node.Value.AsObject;
                return null;
            };

            foreach (var metric in Skill.MetrictList)
                metricUpgradeMap.Add(metric, new MetricUpgrade(metric, lookUp(metric)));
        }

        public Dictionary<SkillMetric, MetricUpgrade>.ValueCollection MetricUpgradeList()
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

        public float OverallWeight()
        {
            float cumulativeLevel = 0;
            foreach (var metricUpgrades in metricUpgradeMap)
                cumulativeLevel += System.Math.Abs(metricUpgrades.Value.Level);
            return cumulativeLevel / 30.0f;
        }

        public float Handicap()
        {
            float cumulativeLevel = 0;
            foreach (var metricUpgrades in metricUpgradeMap)
                cumulativeLevel += metricUpgrades.Value.Level;

            return cumulativeLevel;
        }

        public static float OverallWeight(float cumulativeLevel_)
        {
            return cumulativeLevel_ / 30.0f;
        }
    }
}

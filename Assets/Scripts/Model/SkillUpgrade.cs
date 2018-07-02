using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MetricUpgrade
    {
        public enum SpecializeSign
        {
            Positive,
            Negative,
            None
        }

        public string Name { get { return Json["name"]; } private set { Json["name"] = value; } }
        public string Category { get { return Json["category"]; } private set { Json["category"] = value; } }
        public float Level { get { return Json["level"]; } private set { Json["level"] = value; } }
        public SpecializeSign Sign
        {
            get
            {
                return Level > 0 ? SpecializeSign.Positive : Level < 0 ? SpecializeSign.Negative : SpecializeSign.None;
            }
        }

        public JSONObject Json { get; private set; } = null;
        public SkillMetric Metric { get; private set; } = null;

        public MetricUpgrade(SkillMetric metric_, JSONObject json_)
        {
            Metric = metric_;
            if (json_ !=null)
                Json = json_;
            else
            {
                Json = new JSONObject();
                Category = Metric.Category;
                Name = Metric.Name;
                Level = 0;
            }
        }

        public void Upgrade(SpecializeSign sign_)
        {
            if (sign_ == SpecializeSign.None)
                return;

            if (Sign != SpecializeSign.None && Sign != sign_)
            {
                Debug.Log("Tried upgrading a skill the wrong way");
                return;
            }

            if (sign_ == SpecializeSign.Positive)
                ++Level;
            else
                --Level;
        }
    }

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
                    if (node.Value["category"] == metric_.Category && node.Value["name"] == metric_.Name)
                        return node.Value.AsObject;
                return null;
            };

            foreach (var metric in Skill.MetrictList)
                metricUpgradeMap.Add(metric, new MetricUpgrade(metric, lookUp(metric)));
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
            return cumulativeLevel;
        }

        public float Handicap()
        {
            float cumulativeLevel = 0;
            foreach (var metricUpgrades in metricUpgradeMap)
                cumulativeLevel += metricUpgrades.Value.Level;

            return cumulativeLevel;
        }
    }
}

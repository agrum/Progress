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

        public JSONObject Json { get; private set; }

        public MetricUpgrade(string category_, string name_)
        {
            Json = new JSONObject();
            Category = category_;
            Name = name_;
            Level = 0;
        }

        public MetricUpgrade(JSONObject json_)
        {
            Json = json_;
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
        public Skill Skill
        {
            get
            {
                return skill;
            }
            private set
            {
                skill = value;
            }
        }
        public MetricUpgrade this[string category_, string name_]
        {
            get
            {
                return metricUpgradeMap[SkillMetric.Hash(category_, name_)];
            }
            private set
            {
                metricUpgradeMap[SkillMetric.Hash(category_, name_)] = value;
            }
        }
        public JSONObject Json
        {
            get
            {
                JSONObject json = new JSONObject();

                json["skill"] = skill.Uuid;
                json["upgrades"] = new JSONArray();
                foreach (var upgrade in metricUpgradeMap)
                {
                    if (upgrade.Value.Level != 0)
                        json["upgrades"].Add(upgrade.Value.Json);
                }

                return json;
            }
        }

        private Skill skill = null;
        private Dictionary<string, MetricUpgrade> metricUpgradeMap = new Dictionary<string, MetricUpgrade>();

        public SkillUpgrade(Skill skill_)
        {
            skill = skill_;
        }

        public SkillUpgrade(JSONObject json_)
        {            
            var constellation = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
            Skill = constellation.Skill(json_["skill"]);

            foreach (var node in json_["upgrades"].AsArray)
            {
                var metricUpgrade = new MetricUpgrade(node.Value.AsObject);
                metricUpgradeMap.Add((metricUpgrade.Category + metricUpgrade.Name), metricUpgrade);
            }
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
    }
}

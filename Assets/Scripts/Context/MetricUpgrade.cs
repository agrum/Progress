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
        
        public float Level { get { return Json["level"]; } set { Json["level"] = value; } }

        public JSONObject Json { get; private set; } = null;
        public SkillMetric Metric { get; private set; } = null;

        public MetricUpgrade(SkillMetric metric_, JSONObject json_)
        {
            Metric = metric_;
            if (json_ != null)
                Json = json_;
            else
            {
                Json = new JSONObject();
                Level = 0;
                Json["_id"] = Metric.Json["_id"];
            }
        }

        public SpecializeSign Sign()
        {
            return Sign(Level);
        }

        public float WeightedLevel()
        {
            return WeightedLevel(Level, Metric.UpgCost);
        }
        
        public float Factor()
        {
            return Factor(Level, Metric.UpgType);
        }

        public static SpecializeSign Sign(float level_)
        {
            return level_ > 0 ? SpecializeSign.Positive : level_ < 0 ? SpecializeSign.Negative : SpecializeSign.None;
        }

        public static float WeightedLevel(float level_, float upgradeCost_)
        {
            return level_ / upgradeCost_ / 20.0f;
        }

        public static float Factor(float level_, int upgType_)
        {
            if (upgType_ > 0)
            {
                if (level_ > 0)
                    return level_ / 10.0f;
                else
                    return 1.0f / (1.0f + System.Math.Abs(level_) / 10.0f) - 1.0f;
            }
            else
            {
                if (level_ > 0)
                    return 1.0f / (1.0f + System.Math.Abs(level_) / 10.0f) - 1.0f;
                else
                    return - level_ / 10.0f;
            }
        }
    }
}

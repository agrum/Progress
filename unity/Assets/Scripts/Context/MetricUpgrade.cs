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
        
        public Data.Skill.Metric Metric { get; private set; } = null;
        public int Level { get; private set; } = 0;

        public MetricUpgrade(Data.Skill.Metric metric_, int level_)
        {
            Metric = metric_;
            Level = level_;
        }

        public static implicit operator JSONNode(MetricUpgrade object_)
        {
            JSONObject jObject = new JSONObject();

            jObject["_id"] = object_.Metric.Name;
            jObject["level"] = object_.Level;

            return jObject;
        }

        public SpecializeSign Sign()
        {
            return Sign(Level);
        }

        public double WeightedLevel()
        {
            return WeightedLevel(Level, 1.0);
        }
        
        public double Factor()
        {
            return 1.0; // Factor(Level, Metric.Upgrade);
        }

        public static SpecializeSign Sign(double level_)
        {
            return level_ > 0 ? SpecializeSign.Positive : level_ < 0 ? SpecializeSign.Negative : SpecializeSign.None;
        }

        public static double WeightedLevel(double level_, double upgradeCost_)
        {
            return level_ / upgradeCost_ / 20.0f;
        }

        //public static double Factor(double level_, Data.Skill.Metric.UpgradeType upgradeType_)
        //{
        //    if (upgradeType_.Sign == Data.Skill.Metric.UpgradeType.ESign.Negative)
        //    {
        //        level_ *= -1;
        //    }

        //    if (level_ > 0)
        //        return 1.0 + level_ * upgradeType_.Factor;
        //    else
        //        return 1.0f / (1.0f + System.Math.Abs(level_) * upgradeType_.Factor);
        //}
    }
}

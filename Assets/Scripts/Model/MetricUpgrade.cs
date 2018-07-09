using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MetricUpgrade : Schema.MetricUpgrade
    {
        public delegate void OnLevelChanged();
        public delegate float GetSkillWeight();
        public event OnLevelChanged LevelChanged = delegate { };

        public enum SpecializeSign
        {
            Positive,
            Negative,
            None
        }
        public SpecializeSign Sign
        {
            get
            {
                return TemporaryLevel > 0 ? SpecializeSign.Positive : TemporaryLevel < 0 ? SpecializeSign.Negative : SpecializeSign.None;
            }
        }

        public SkillMetric Metric { get; private set; } = null;
        public float TemporaryLevel { get; private set; } = 0;
        private GetSkillWeight getSkillWeight;

        public float WeightedLevel { get { return TemporaryLevel / Metric.UpgCost / 20.0f; } }

        private float startingLevel = 0;

        public MetricUpgrade(SkillMetric metric_, JSONObject json_, GetSkillWeight delegate_) : base(json_)
        {
            Metric = metric_;
            getSkillWeight = delegate_;
            if (json_ != null)
                Json = json_;
            else
            {
                Json = new JSONObject();
                Category = Metric.Category;
                Name = Metric.Name;
                Level = 0;
            }

            Verify();
            Reset();
        }

        ~MetricUpgrade()
        {
            LevelChanged = null;
        }

        public void Upgrade()
        {
            if (startingLevel < 0 && startingLevel > TemporaryLevel)
                throw new WestException("Tried upgrading a skill the wrong way");

            if (WeightedLevel >= 1.0f)
                throw new WestException("Tried upgrading a capped metric");

            if (startingLevel >= 0 && getSkillWeight() >= 1.0f)
                throw new WestException("Tried upgrading a capped skill");
            
            ++TemporaryLevel;
            LevelChanged();
        }

        public void Downgrade()
        {
            if (startingLevel > 0 && startingLevel < TemporaryLevel)
                throw new WestException("Tried upgrading a skill the wrong way");

            if (WeightedLevel <= -1.0f)
                throw new WestException("Tried upgrading a capped metric");

            if (startingLevel <= 0 && getSkillWeight() >= 1.0f)
                throw new WestException("Tried upgrading a capped skill");

            --TemporaryLevel;
            LevelChanged();
        }

        public void Reset()
        {
            startingLevel = Level;
            TemporaryLevel = Level;
        }

        public void Save()
        {
            Level = TemporaryLevel;
            Reset();
        }

        public float Factor
        {
            get
            {
                return (Metric.UpgType > 0) ^ (TemporaryLevel < 0)
                ? TemporaryLevel / 10.0f
                : ((Metric.UpgType == 1) ? 1 : -1) * (1.0f - 1.0f / (1.0f + System.Math.Abs(TemporaryLevel) / 10.0f));
            }
        }
    }
}

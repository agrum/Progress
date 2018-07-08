using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MetricUpgrade : Schema.MetricUpgrade
    {
        public delegate void OnLevelChanged();
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

        private float startingLevel = 0;

        public MetricUpgrade(SkillMetric metric_, JSONObject json_) : base(json_)
        {
            Metric = metric_;
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

        public void Upgrade()
        {
            if (startingLevel == 0 || (startingLevel <= TemporaryLevel && startingLevel > 0))
            {
                ++TemporaryLevel;
                LevelChanged();
                return;
            }

            throw new WestException("Tried upgrading a skill the wrong way");
        }

        public void Downgrade()
        {
            if (startingLevel == 0 || (startingLevel >= TemporaryLevel && startingLevel < 0))
            {
                --TemporaryLevel;
                LevelChanged();
                return;
            }

            throw new WestException("Tried upgrading a skill the wrong way");
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
    }
}

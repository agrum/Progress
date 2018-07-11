using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializationField : IBase
    {
        public event OnVoidDelegate LevelChanged = delegate { };

        private Model.MetricUpgrade metricUpgrade = null;
        private Model.MetricUpgrade referenceMetricUpgrade = null;
        private bool editable = false;

        public bool IsPreviewing { get; private set; } = false;
        public float PrePreviewLevel { get; private set; } = 0;
        public float PrePreviewFactor { get; private set; } = 0;

        public SkillSpecializationField(Model.MetricUpgrade upgrade_, bool editable_)
        {
            Debug.Assert(upgrade_ != null);

            metricUpgrade = upgrade_;
            referenceMetricUpgrade = new Model.MetricUpgrade(metricUpgrade.Metric, metricUpgrade.Json, () => { return 0; });
            editable = editable_;

            metricUpgrade.LevelChanged += OnLevelChanged;
        }

        ~SkillSpecializationField()
        {
            metricUpgrade.LevelChanged -= OnLevelChanged;
            metricUpgrade = null;
        }

        public bool Editable()
        {
            return editable;
        }

        public Model.MetricUpgrade.SpecializeSign Sign()
        {
            return metricUpgrade.Sign;
        }

        public float Level()
        {
            return referenceMetricUpgrade.WeightedLevel;
        }

        public float TemporaryLevel()
        {
            return metricUpgrade.WeightedLevel;
        }

        public float Factor()
        {
            return referenceMetricUpgrade.Factor;
        }

        public float TemporaryFactor()
        {
            return metricUpgrade.Factor;
        }

        public string Category()
        {
            return metricUpgrade.Category;
        }

        public string Name()
        {
            return metricUpgrade.Name;
        }

        public void PreviewUpgrade(bool enabled)
        {
            if (enabled)
            {
                IsPreviewing = true;
                Upgrade();
            }
            else if (IsPreviewing)
            {
                IsPreviewing = false;
                Downgrade();
            }
        }

        public void PreviewDowngrade(bool enabled)
        {
            if (enabled)
            {
                IsPreviewing = true;
                Downgrade();
            }
            else if (IsPreviewing)
            {
                IsPreviewing = false;
                Upgrade();
            }
        }

        public void Buy()
        {
            metricUpgrade.Save();
        }

        public void Upgrade()
        {
            if (!editable)
                return;

            try
            {
                PrePreviewLevel = TemporaryLevel();
                PrePreviewFactor = TemporaryFactor();
                metricUpgrade.Upgrade();
            }
            catch (WestException e)
            {
                Debug.Log(e.Message);
                IsPreviewing = false;
            }
        }

        public void Downgrade()
        {
            if (!editable)
                return;

            try
            {
                PrePreviewLevel = TemporaryLevel();
                PrePreviewFactor = TemporaryFactor();
                metricUpgrade.Downgrade();
            }
            catch (WestException e)
            {
                Debug.Log(e.Message);
                IsPreviewing = false;
            }
        }

        private void OnLevelChanged()
        {
            LevelChanged();
        }
    }
}

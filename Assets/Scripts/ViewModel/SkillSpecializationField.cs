using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializationField : IBase
    {
        public event OnVoidDelegate LevelChanged = delegate { };

        private Model.MetricUpgrade metricUpgrade = null;
        private Model.MetricUpgrade referenceMetricUpgrade = null;
        private bool editable = false;
        private bool isPreviewing = false;

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
                isPreviewing = true;
                _Upgrade();
            }
            else if (isPreviewing)
            {
                _Downgrade();
                isPreviewing = false;
            }
        }

        public void PreviewDowngrade(bool enabled)
        {
            if (enabled)
            {
                isPreviewing = true;
                _Downgrade();
            }
            else if (isPreviewing)
            {
                _Upgrade();
                isPreviewing = false;
            }
        }

        public void Upgrade()
        {
            isPreviewing = false;
        }

        public void Downgrade()
        {
            isPreviewing = false;
        }

        public void Buy()
        {
            metricUpgrade.Save();
        }

        private void _Upgrade()
        {
            if (!editable)
                return;

            try
            {
                metricUpgrade.Upgrade();
            }
            catch (WestException e)
            {
                Debug.Log(e.Message);
                isPreviewing = false;
            }
        }

        private void _Downgrade()
        {
            if (!editable)
                return;

            try
            {
                metricUpgrade.Downgrade();
            }
            catch (WestException e)
            {
                Debug.Log(e.Message);
                isPreviewing = false;
            }
        }

        private void OnLevelChanged()
        {
            LevelChanged();
        }
    }
}

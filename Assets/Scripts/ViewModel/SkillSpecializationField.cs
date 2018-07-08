using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializationField : IBase
    {
        public event OnVoidDelegate LevelChanged = delegate { };

        private Model.MetricUpgrade metricUpgrade = null;
        private bool editable = false;
        private bool isPreviewing = false;

        public SkillSpecializationField(Model.MetricUpgrade upgrade_, bool editable_)
        {
            Debug.Assert(upgrade_ != null);

            metricUpgrade = upgrade_;
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
            return metricUpgrade.Level / 20.0f;
        }

        public float TemporaryLevel()
        {
            return metricUpgrade.TemporaryLevel / 20.0f;
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
                Upgrade();
            }
            else if (isPreviewing)
            {
                Downgrade();
                isPreviewing = false;
            }
        }

        public void PreviewDowngrade(bool enabled)
        {
            if (enabled)
            {
                isPreviewing = true;
                Downgrade();
            }
            else if (isPreviewing)
            {
                Upgrade();
                isPreviewing = false;
            }
        }

        public void Upgrade()
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

        public void Downgrade()
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

        public void Buy()
        {
            metricUpgrade.Save();
        }

        private void OnLevelChanged()
        {
            LevelChanged();
        }
    }
}

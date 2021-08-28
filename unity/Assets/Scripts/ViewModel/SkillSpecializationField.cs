using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializationField : IBase
    {
        public event OnVoidDelegate LevelChanged = delegate { };

        private Model.SkillSpecializer specializer = null;
        private Model.MetricUpgrade metricUpgrade = null;
        private bool editable = false;

        public bool IsPreviewing { get; private set; } = false;
        public double PrePreviewLevel { get; private set; } = 0;
        public double PrePreviewFactor { get; private set; } = 0;

        public SkillSpecializationField(Model.SkillSpecializer specializer_, Model.MetricUpgrade upgrade_, bool editable_)
        {
            Debug.Assert(specializer_ != null);
            Debug.Assert(upgrade_ != null);

            specializer = specializer_;
            metricUpgrade = upgrade_;
            editable = editable_;

            specializer.SkillSpecialized += OnLevelChanged;
        }

        ~SkillSpecializationField()
        {
            specializer.SkillSpecialized -= OnLevelChanged;
            metricUpgrade = null;
        }

        public bool Editable()
        {
            return editable;
        }

        public Model.MetricUpgrade.SpecializeSign Sign()
        {
            return specializer.Sign(metricUpgrade);
        }

        public double Level()
        {
            return metricUpgrade.WeightedLevel();
        }

        public double TemporaryLevel()
        {
            return specializer.WeightedLevel(metricUpgrade);
        }

        public double Factor()
        {
            return metricUpgrade.Factor();
        }

        public double TemporaryFactor()
        {
            return specializer.Factor(metricUpgrade);
        }

        public Data.Skill.Metric.ECategory Category()
        {
            return metricUpgrade.Metric.Category;
        }

        public Data.NamedHash Name()
        {
            return metricUpgrade.Metric.Name;
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
            //specializer.Save(metricUpgrade);
        }

        public void Upgrade()
        {
            if (!editable)
                return;

            try
            {
                PrePreviewLevel = TemporaryLevel();
                PrePreviewFactor = TemporaryFactor();
                specializer.Upgrade(metricUpgrade);
            }
            catch (WestException e)
            {
                Debug.Log(e.Message);
                IsPreviewing = false;
                LevelChanged();
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
                specializer.Downgrade(metricUpgrade);
            }
            catch (WestException e)
            {
                Debug.Log(e.Message);
                IsPreviewing = false;
                LevelChanged();
            }
        }

        private void OnLevelChanged()
        {
            LevelChanged();
        }
    }
}

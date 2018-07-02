using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializationField : IBase
    {
        public event OnVoidDelegate SpecLevelChanged = delegate { };
        
        private Model.MetricUpgrade upgrade = null;
        private bool editable = false;

        public SkillSpecializationField(Model.MetricUpgrade upgrade_, bool editable_)
        {
            Debug.Assert(upgrade_ != null);
            
            upgrade = upgrade_;
            editable = editable_;
        }

        public bool Editable()
        {
            return editable;
        }

        public Model.MetricUpgrade.SpecializeSign Sign()
        {
            return upgrade.Sign;
        }

        public float SpecLevel()
        {
            return upgrade.Level / 20.0f;
        }

        public string Category()
        {
            return upgrade.Category;
        }

        public string Name()
        {
            return upgrade.Name;
        }

        public float NextSpecLevel(Model.MetricUpgrade.SpecializeSign sign_)
        {
            if (sign_ == Model.MetricUpgrade.SpecializeSign.None)
                return 0.0f;

            if (upgrade.Sign != Model.MetricUpgrade.SpecializeSign.None && upgrade.Sign != sign_)
            {
                Debug.Log("Tried upgrading a skill the wrong way");
                return 0.0f;
            }

            if ((System.Math.Abs(upgrade.Level) + 0.5f) / upgrade.Metric.UpgCost > 20.0f)
            {
                Debug.Log("Tried upgrading a skill too far past limit");
                return 0.0f;
            }
            
            return (System.Math.Abs(upgrade.Level) + 1.0f) / upgrade.Metric.UpgCost / 20.0f;
        }

        public void Buy(Model.MetricUpgrade.SpecializeSign sign_)
        {
            float nextLevel = NextSpecLevel(sign_);

            if (nextLevel == 0.0f)
                return;

            upgrade.Upgrade(sign_);
        }
    }
}

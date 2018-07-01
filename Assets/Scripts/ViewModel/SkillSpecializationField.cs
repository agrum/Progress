using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializationField
    {
        public event OnVoidDelegate SpecLevelChanged = delegate { };
        
        private Model.SkillMetric metric = null;
        private Model.MetricUpgrade upgrade = null;

        public SkillSpecializationField(Model.SkillMetric metric_, Model.MetricUpgrade upgrade_)
        {
            Debug.Assert(metric_ != null);
            Debug.Assert(upgrade_ != null);

            metric = metric_;
            upgrade = upgrade_;
        }

        public bool Editable()
        {
            return true;
        }

        public Model.MetricUpgrade.SpecializeSign Sign()
        {
            return upgrade.Sign;
        }

        public float SpecLevel()
        {
            return upgrade.Level / 20.0f;
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

            if ((System.Math.Abs(upgrade.Level) + 0.5f) / metric.UpgCost > 20.0f)
            {
                Debug.Log("Tried upgrading a skill too far past limit");
                return 0.0f;
            }
            
            return (System.Math.Abs(upgrade.Level) + 1.0f) / metric.UpgCost / 20.0f;
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

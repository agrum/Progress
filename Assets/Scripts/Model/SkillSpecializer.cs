using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class SkillSpecializer
    {
        public delegate void OnSkillSpecialized();
        public event OnSkillSpecialized SkillSpecialized;

        private Champion champion = null;
        private SkillUpgrade skillUpgrade = null;

        private Dictionary<MetricUpgrade, int> temporaryUpgradeMap = new Dictionary<MetricUpgrade, int>();
        private int spentPoint = 0;

        public Skill Skill()
        {
            return skillUpgrade.Skill;
        }

        public SkillSpecializer(Champion champion_, SkillUpgrade skillUpgrade_)
        {
            champion = champion_;
            skillUpgrade = skillUpgrade_;
        }

        public MetricUpgrade.SpecializeSign Sign(MetricUpgrade upgrade_)
        {
            if (temporaryUpgradeMap.ContainsKey(upgrade_))
                return MetricUpgrade.Sign(temporaryUpgradeMap[upgrade_] + upgrade_.Level);
            return MetricUpgrade.Sign(upgrade_.Level);
        }

        public float Level(MetricUpgrade upgrade_)
        {
            if (temporaryUpgradeMap.ContainsKey(upgrade_))
                return temporaryUpgradeMap[upgrade_] + upgrade_.Level;
            return upgrade_.Level;
        }

        public float WeightedLevel(MetricUpgrade upgrade_)
        {
            if (temporaryUpgradeMap.ContainsKey(upgrade_))
                return MetricUpgrade.WeightedLevel(temporaryUpgradeMap[upgrade_] + upgrade_.Level, upgrade_.Metric.UpgCost);
            return upgrade_.WeightedLevel();
        }

        public float Factor(MetricUpgrade upgrade_)
        {
            if (temporaryUpgradeMap.ContainsKey(upgrade_))
                return MetricUpgrade.Factor(temporaryUpgradeMap[upgrade_] + upgrade_.Level, upgrade_.Metric.UpgType);
            return upgrade_.Factor();
        }

        public void Upgrade(MetricUpgrade upgrade_)
        {
            if (!temporaryUpgradeMap.ContainsKey(upgrade_))
                temporaryUpgradeMap.Add(upgrade_, 0);

            var metricUpgrade = skillUpgrade[upgrade_.Metric];
            var temporaryUpgrade = temporaryUpgradeMap[upgrade_];
            var temporaryLevel = metricUpgrade.Level + temporaryUpgrade;

            if (metricUpgrade.Level < 0 && metricUpgrade.Level > temporaryLevel)
                throw new WestException("Tried upgrading a skill the wrong way");

            if (MetricUpgrade.WeightedLevel(temporaryLevel, upgrade_.Metric.UpgCost) >= 1.0f)
                throw new WestException("Tried upgrading a capped metric");

            if (metricUpgrade.Level >= 0 && OverallWeight() >= 1.0f)
                throw new WestException("Tried upgrading a capped skill");

            ++temporaryUpgradeMap[upgrade_];
            SkillSpecialized();
        }

        public void Downgrade(MetricUpgrade upgrade_)
        {
            if (!temporaryUpgradeMap.ContainsKey(upgrade_))
                temporaryUpgradeMap.Add(upgrade_, 0);

            var metricUpgrade = skillUpgrade[upgrade_.Metric];
            var temporaryUpgrade = temporaryUpgradeMap[upgrade_];
            var temporaryLevel = metricUpgrade.Level + temporaryUpgrade;

            if (metricUpgrade.Level > 0 && metricUpgrade.Level < temporaryLevel)
                throw new WestException("Tried upgrading a skill the wrong way");

            if (MetricUpgrade.WeightedLevel(temporaryLevel, upgrade_.Metric.UpgCost) <= -1.0f)
                throw new WestException("Tried upgrading a capped metric");

            if (metricUpgrade.Level <= 0 && OverallWeight() >= 1.0f)
                throw new WestException("Tried upgrading a capped skill");

            --temporaryUpgradeMap[upgrade_];
            SkillSpecialized();
        }

        public float OverallWeight()
        {
            float cumulativeLevel = 0;
            foreach (var upgrade in skillUpgrade.MetricUpgradeList())
            {
                var localLevel = upgrade.Level;
                if (temporaryUpgradeMap.ContainsKey(upgrade))
                    localLevel += temporaryUpgradeMap[upgrade];
                cumulativeLevel = Math.Abs(localLevel);
            }
            return SkillUpgrade.OverallWeight(cumulativeLevel);
        }

        public float Handicap()
        {
            float cumulativeLevel = 0;
            foreach (var upgrade in skillUpgrade.MetricUpgradeList())
            {
                var localLevel = upgrade.Level;
                if (temporaryUpgradeMap.ContainsKey(upgrade))
                    localLevel += temporaryUpgradeMap[upgrade];
                cumulativeLevel = localLevel;
            }
            return cumulativeLevel;
        }
    }
}

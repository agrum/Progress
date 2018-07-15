using System;
using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class SkillSpecializer
    {
        public delegate void OnSkillSpecialized();
        public event OnSkillSpecialized SkillSpecialized = delegate { };
        public delegate void OnSkillSpecializationSaved();
        public event OnSkillSpecializationSaved SkillSpecializationSaved = delegate { };

        private Champion champion = null;
        private SkillUpgrade skillUpgrade = null;

        private Dictionary<MetricUpgrade, int> temporaryUpgradeMap = new Dictionary<MetricUpgrade, int>();
        private int pointsSpent = 0;

        public Skill Skill()
        {
            return skillUpgrade.Skill;
        }

        public SkillSpecializer(Champion champion_, SkillUpgrade skillUpgrade_)
        {
            champion = champion_;
            skillUpgrade = skillUpgrade_;
        }

        ~SkillSpecializer()
        {
            SkillSpecialized = null;
            SkillSpecializationSaved = null;
        }

        public int SpecializationPoints()
        {
            return champion.SpecializationPoints - pointsSpent;
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

            if (metricUpgrade.Level < 0 && temporaryUpgrade == 0)
                throw new WestException("Tried upgrading a skill the wrong way");

            if (MetricUpgrade.WeightedLevel(temporaryLevel, upgrade_.Metric.UpgCost) >= 1.0f)
                throw new WestException("Tried upgrading a capped metric");

            if (metricUpgrade.Level >= 0 && OverallWeight() >= 1.0f)
                throw new WestException("Tried upgrading a capped skill");

            if (temporaryLevel >= 0 && SpecializationPoints() == 0)
                throw new WestException("No more spec poitns to spend");

            pointsSpent += (temporaryLevel >= 0) ? 1 : -1;
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

            if (metricUpgrade.Level > 0 && temporaryUpgrade == 0)
                throw new WestException("Tried upgrading a skill the wrong way");

            if (MetricUpgrade.WeightedLevel(temporaryLevel, upgrade_.Metric.UpgCost) <= -1.0f)
                throw new WestException("Tried upgrading a capped metric");

            if (metricUpgrade.Level <= 0 && OverallWeight() >= 1.0f)
                throw new WestException("Tried upgrading a capped skill");

            if (temporaryLevel <= 0 && SpecializationPoints() == 0)
                throw new WestException("No more spec poitns to spend");

            pointsSpent += (temporaryLevel <= 0) ? 1 : -1;
            --temporaryUpgradeMap[upgrade_];
            SkillSpecialized();
        }

        public void Apply()
        {
            if (pointsSpent < 0)
                throw new WestException("Can't have a negative spent");

            int upgradeSum = 0;
            foreach (var upgradePair in temporaryUpgradeMap)
            {
                //skillUpgrade[upgradePair.Key.Metric].Level += upgradePair.Value;
                upgradeSum += Math.Abs(upgradePair.Value);
            }

            if (upgradeSum != pointsSpent)
                throw new WestException("Mismatch upgrade count and poitn spent");

            //champion.SpecializationPoints -= pointsSpent;

            JSONObject requestField = new JSONObject();
            JSONArray upgradeArray = new JSONArray();

            foreach (var upgradePair in temporaryUpgradeMap)
            {
                if (upgradePair.Value == 0)
                    continue;

                JSONObject upgrade = new JSONObject();
                upgrade["metric"] = upgradePair.Key.Json["_id"];
                upgrade["diff"] = upgradePair.Value;
                upgradeArray.Add(upgrade);
            }
            requestField["upgrades"] = upgradeArray;
            requestField["skill"] = Skill().Json["_id"];
            requestField["pointsSpent"] = pointsSpent;

            var loadingScreen = App.Resource.Prefab.LoadingCanvas();
            var request = App.Server.Request(
                HTTPMethods.Post,
                "champion/" + champion.Json["_id"] + "/skillUpgrade",
                (JSONNode json_) =>
                {
                    Debug.Log("Success");
                    champion.Unload();
                    SkillSpecializationSaved();
                },
                (JSONNode json_) =>
                {
                    Debug.Log("Fail");
                    Debug.Log(json_.ToString());
                    GameObject.Destroy(loadingScreen.gameObject);
                    App.Resource.Prefab.Popup().Setup("Error", "Skill upgrade failed.");
                });
            request.AddField("report", requestField.ToString());
            request.Send();
        }

        public float OverallWeight()
        {
            float cumulativeLevel = 0;
            foreach (var upgrade in skillUpgrade.MetricUpgradeList())
            {
                var localLevel = upgrade.Level;
                if (temporaryUpgradeMap.ContainsKey(upgrade))
                    localLevel += temporaryUpgradeMap[upgrade];
                cumulativeLevel += Math.Abs(localLevel);
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
                cumulativeLevel += localLevel;
            }
            return cumulativeLevel;
        }
    }
}

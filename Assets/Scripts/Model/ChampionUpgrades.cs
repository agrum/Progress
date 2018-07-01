﻿using System.Collections.Generic;
using SimpleJSON;

namespace Assets.Scripts.Model
{
    public class ChampionUpgrades
    {
        public SkillUpgrade this[Skill skill_]
        {
            get
            {
                var skillUpgrade = skillUpgradeMap[skill_];
                if (skillUpgrade == null)
                {
                    skillUpgrade =  new SkillUpgrade(skill_);
                    skillUpgradeMap[skill_] = skillUpgrade;
                }
                return skillUpgrade;
            }
        }
        public JSONArray Json
        {
            get
            {
                JSONArray json = new JSONArray();

                foreach (var skillUpgrade in skillUpgradeMap)
                {
                    if (skillUpgrade.Value.IsValid())
                    {
                        json.Add(skillUpgrade.Value.Json);
                    }
                }

                return json;
            }
        }

        private Dictionary<Skill, SkillUpgrade> skillUpgradeMap = new Dictionary<Skill, SkillUpgrade>();

        public ChampionUpgrades(JSONArray json_)
        {
            foreach (var node in json_["skillUpgrades"].AsArray)
            {
                var skillUpgrade = new SkillUpgrade(node.Value.AsObject);
                skillUpgradeMap.Add(skillUpgrade.Skill, skillUpgrade);
            }

        }
    }
}

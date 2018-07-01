using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializer
    {
        public event OnElementAdded NodeAdded = delegate { };
        public event OnElementAdded SpecializerFieldAdded = delegate { };

        private Model.Skill skill = null;
        private Model.SkillUpgrade skillUpgrade = null;

        public SkillSpecializer(Model.Skill skill_)
        {
            Debug.Assert(skill_ != null);

            skill = skill_;
            skillUpgrade = App.Content.Account.ActiveChampion.Upgrades[skill];
        }

        ~SkillSpecializer()
        {
            SpecializerFieldAdded = null;
        }

        public string Name()
        {
            return skill.Json["name"];
        }

        public float Handicap()
        {
            return skillUpgrade.Handicap();
        }

        public float OverallWeight()
        {
            return skillUpgrade.Overallweight();
        }
    }
}

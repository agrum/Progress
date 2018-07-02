using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializer
    {
        public event OnElementAdded NodeAdded = delegate { };
        public event OnElementAdded SpecializerFieldAdded = delegate { };

        private Model.Skill skill = null;
        private Model.SkillUpgrade skillUpgrade = null;
        private Model.Json scale = null;

        public SkillSpecializer(Model.Skill skill_)
        {
            Debug.Assert(skill_ != null);

            skill = skill_;
            skillUpgrade = App.Content.Account.ActiveChampion.Upgrades[skill];
            scale = new Model.Json();
            scale["scale"] = 1.0f;
        }

        public void Setup()
        { 
            NodeAdded(() =>
            {
                return new NodeEmpty(
                    skill,
                    scale,
                    skill.Material,
                    new Vector2(0, 0));
            });

            foreach (var metric in skill.MetrictList)
            {
                SpecializerFieldAdded(() =>
                {
                    return new SkillSpecializationField(skillUpgrade[metric]);
                });
            }
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
            return skillUpgrade.OverallWeight();
        }
    }
}

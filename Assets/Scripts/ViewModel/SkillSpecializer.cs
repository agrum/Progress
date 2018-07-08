using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializer
    {
        public event OnElementAdded NodeAdded = delegate { };
        public event OnElementAdded SpecializerFieldAdded = delegate { };
        public event OnVoidDelegate SkillUpgraded = delegate { };

        private Model.SkillUpgrade skillUpgrade = null;
        private bool editable = false;
        private Model.Json scale = null;

        public SkillSpecializer(Model.SkillUpgrade skillUpgrade_, bool editable_)
        {
            Debug.Assert(skillUpgrade_ != null);

            skillUpgrade = skillUpgrade_;
            editable = editable_;
            scale = new Model.Json();
            scale["scale"] = 1.0f;
        }

        public void Setup()
        { 
            NodeAdded(() =>
            {
                return new NodeEmpty(
                    skillUpgrade.Skill,
                    scale,
                    skillUpgrade.Skill.Material,
                    new Vector2(0, 0));
            });

            foreach (var metric in skillUpgrade.Skill.MetrictList)
            {
                SpecializerFieldAdded(() =>
                {
                    return new SkillSpecializationField(
                        skillUpgrade[metric],
                        editable);
                });
            }
        }

        ~SkillSpecializer()
        {
            NodeAdded = null;
            SpecializerFieldAdded = null;
            SkillUpgraded = null;
        }

        public string Name()
        {
            return skillUpgrade.Skill.Json["name"];
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

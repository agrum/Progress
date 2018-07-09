using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializer
    {
        public event OnElementAdded NodeAdded = delegate { };
        public event OnElementAdded SpecializerFieldAdded = delegate { };
        public event OnVoidDelegate SkillUpgraded = delegate { };

        private Model.SkillUpgrade skillUpgrade = null;
        private Model.SkillUpgrade referenceSkillUpgrade = null;
        private bool editable = false;
        private Model.HoveredSkill hovered = null;
        private Model.Json scale = null;

        public SkillSpecializer(Model.SkillUpgrade skillUpgrade_, bool editable_, Model.HoveredSkill hovered_)
        {
            Debug.Assert(skillUpgrade_ != null);

            skillUpgrade = skillUpgrade_;
            referenceSkillUpgrade = new Model.SkillUpgrade(skillUpgrade.Skill);
            editable = editable_;
            hovered = hovered_;
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
                    var viewModel = new SkillSpecializationField(
                        skillUpgrade[metric],
                        editable);
                    viewModel.LevelChanged += OnMetricLevelChanged;
                    return viewModel;
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
            return referenceSkillUpgrade.Handicap();
        }

        public float PreviewHandicap()
        {
            return skillUpgrade.Handicap();
        }

        public float OverallWeight()
        {
            return referenceSkillUpgrade.OverallWeight();
        }

        public float OverallPreviewWeight()
        {
            return skillUpgrade.OverallWeight();
        }

        private void OnMetricLevelChanged()
        {
            hovered.Skill = hovered.Skill;
            SkillUpgraded();
        }
    }
}

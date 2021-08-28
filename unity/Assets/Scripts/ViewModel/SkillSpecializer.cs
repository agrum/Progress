using UnityEngine;

namespace Assets.Scripts.ViewModel
{
    public class SkillSpecializer
    {
        public event OnElementAdded NodeAdded = delegate { };
        public event OnElementAdded SpecializerFieldAdded = delegate { };
        public event OnVoidDelegate SkillUpgraded = delegate { };

        private Model.SkillSpecializer specializer = null;
        private Model.SkillUpgrade skillUpgrade = null;
        private bool editable = false;
        private Model.HoveredSkill hovered = null;
        private Model.Json scale = null;

        public SkillSpecializer(Model.SkillSpecializer specializer_, Model.SkillUpgrade skillUpgrade_, bool editable_, Model.HoveredSkill hovered_)
        {
            Debug.Assert(specializer_ != null);
            Debug.Assert(skillUpgrade_ != null);

            specializer = specializer_;
            skillUpgrade = skillUpgrade_;
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

            foreach (var metric in skillUpgrade.Skill.Metrics)
            {
                SpecializerFieldAdded(() =>
                {
                    var viewModel = new SkillSpecializationField(
                        specializer,
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
            return skillUpgrade.Skill.Name.String;
        }

        public double Handicap()
        {
            return skillUpgrade.Handicap();
        }

        public double PreviewHandicap()
        {
            return specializer.Handicap();
        }

        public double OverallWeight()
        {
            return skillUpgrade.OverallWeight();
        }

        public double OverallPreviewWeight()
        {
            return specializer.OverallWeight();
        }

        private void OnMetricLevelChanged()
        {
            hovered.Skill = hovered.Skill;
            SkillUpgraded();
        }
    }
}

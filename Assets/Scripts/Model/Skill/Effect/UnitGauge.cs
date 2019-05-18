using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class UnitGauge : Base
    {
        public enum ETarget
        {
            Value,
            Capacity,
        }

        public enum ECategory
        {
            None = 0,
            Physical = 1,
            Magical = 2,
            Heal = 4,
            True = 8,
        }
        
        public NamedHash Gauge { get; private set; }
        public ETarget Target { get; private set; }
        public ECategory Category { get; private set; }
        public Skill.UnitGauge.EInputType InputType { get; private set; }
        public SkillMetricReference Reference { get; private set; }

        public UnitGauge(
            string name_,
            Skill.UnitGauge.EType gauge_,
            ETarget target_,
            ECategory category_,
            Skill.UnitGauge.EInputType inputType_,
            SkillMetricReference reference_,
            ESubject from_ = ESubject.Trigger,
            ESubject to_ = ESubject.Target)
            : base(name_, from_, to_)
        {
            Gauge = gauge_;
            Target = target_;
            Category = category_;
            InputType = inputType_;
            Reference = reference_;
        }

        public UnitGauge(
            JSONNode jNode_)
            : base(jNode_)
        {
            Gauge = jNode_["Gauge"];
            Target = (ETarget)Enum.Parse(typeof(ETarget), jNode_["target"]);
            Category = (ECategory)Enum.Parse(typeof(ECategory), jNode_["category"]);
            InputType = (Skill.UnitGauge.EInputType)Enum.Parse(typeof(Skill.UnitGauge.EInputType), jNode_["input"]);
            Reference = jNode_["reference"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Gauge"] = Gauge;
            jObject["Target"] = Target.ToString("G");
            jObject["Category"] = Category.ToString("G");
            jObject["InputType"] = InputType.ToString("G");
            jObject["Reference"] = Reference;
            return jObject;
        }
    }
}

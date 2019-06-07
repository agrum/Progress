using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Gauge : Base
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
        
        public NamedHash Hash { get; private set; }
        public ETarget Target { get; private set; }
        public ECategory Category { get; private set; }
        public Model.Skill.Unit.Gauge.EInputType InputType { get; private set; }
        public MetricReference Reference { get; private set; }

        public Gauge(
            Id id_,
            NamedHash hash_,
            ETarget target_,
            ECategory category_,
            Model.Skill.Unit.Gauge.EInputType inputType_,
            MetricReference reference_)
            : base(id_)
        {
            Hash = hash_;
            Target = target_;
            Category = category_;
            InputType = inputType_;
            Reference = reference_;
        }

        public Gauge(
            JSONNode jNode_)
            : base(jNode_)
        {
            Hash = jNode_["Hash"];
            Target = (ETarget)Enum.Parse(typeof(ETarget), jNode_["Target"]);
            Category = (ECategory)Enum.Parse(typeof(ECategory), jNode_["Category"]);
            InputType = (Model.Skill.Unit.Gauge.EInputType)Enum.Parse(typeof(Model.Skill.Unit.Gauge.EInputType), jNode_["InputType"]);
            Reference = jNode_["Reference"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Hash"] = Hash;
            jObject["Target"] = Target.ToString("G");
            jObject["Category"] = Category.ToString("G");
            jObject["InputType"] = InputType.ToString("G");
            jObject["Reference"] = Reference;
            return jObject;
        }
    }
}

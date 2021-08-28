using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Gauge : Base
    {
        public enum ETarget
        {
            Capacity,
            CapacityAndProportionalValue,
            CapacityAndValue
        }
        
        public NamedHash Hash { get; private set; }
        public ETarget Target { get; private set; }
        public Data.Skill.Unit.Stat.ECategory Category { get; private set; }
        public Data.Skill.Unit.Gauge.EInputType InputType { get; private set; }
        public MetricReference Reference { get; private set; }

        public Gauge(
            Id id_,
            NamedHash hash_,
            ETarget target_,
            Data.Skill.Unit.Stat.ECategory category_,
            Data.Skill.Unit.Gauge.EInputType inputType_,
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
            Hash = jNode_["hash"];
            Target = Serializer.ReadEnum<ETarget>(jNode_["target"]);
            Category = Serializer.ReadEnum<Data.Skill.Unit.Stat.ECategory>(jNode_["category"]);
            InputType = Serializer.ReadEnum<Data.Skill.Unit.Gauge.EInputType>(jNode_["target"]);
            Reference = jNode_["reference"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["hash"] = Hash;
            jObject["target"] = Target;
            jObject["category"] = Category;
            jObject["inputType"] = InputType;
            jObject["reference"] = Reference;
            return jObject;
        }
    }
}

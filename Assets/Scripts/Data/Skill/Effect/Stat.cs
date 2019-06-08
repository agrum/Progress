using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Stat : Base
    {
        public NamedHash Hash { get; private set; }
        public Model.Skill.Unit.Stat.EInputType InputType { get; private set; }
        public MetricReference Reference { get; private set; }

        public Stat(
            Id id_,
            NamedHash hash_,
            Model.Skill.Unit.Stat.EInputType inputType_,
            MetricReference reference_)
            : base(id_)
        {
            Hash = hash_;
            InputType = inputType_;
            Reference = reference_;
        }

        public Stat(
            JSONNode jNode_)
            : base(jNode_)
        {
            Hash = jNode_["Hash"];
            InputType = Serializer.ReadEnum<Model.Skill.Unit.Stat.EInputType>(jNode_["InputType"]);
            Reference = jNode_["Reference"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Hash"] = Hash;
            jObject["InputType"] = InputType;
            jObject["Reference"] = Reference;
            return jObject;
        }
    }
}

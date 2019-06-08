using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Cooldown : Base
    {
        public NamedHash Hash { get; private set; }
        public MetricReference Reference { get; private set; }

        public Cooldown(
            Id id_,
            NamedHash hash_,
            MetricReference reference_)
            : base(id_)
        {
            Hash = hash_;
            Reference = reference_;
        }

        public Cooldown(
            JSONNode jNode_)
            : base(jNode_)
        {
            Hash = jNode_["Hash"];
            Reference = jNode_["Reference"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Hash"] = Hash;
            jObject["Reference"] = Reference;
            return jObject;
        }
    }
}

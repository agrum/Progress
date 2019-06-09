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
        public Data.Skill.Unit.Stat.ECategory Category { get; private set; }
        public MetricReference Reference { get; private set; }

        public Stat(
            Id id_,
            NamedHash hash_,
            Data.Skill.Unit.Stat.ECategory category_,
            MetricReference reference_)
            : base(id_)
        {
            Hash = hash_;
            Category = category_;
            Reference = reference_;
        }

        public Stat(
            JSONNode jNode_)
            : base(jNode_)
        {
            Hash = jNode_["Hash"];
            Category = Serializer.ReadEnum<Data.Skill.Unit.Stat.ECategory>(jNode_["Category"]);
            Reference = jNode_["Reference"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Hash"] = Hash;
            jObject["Category"] = Category;
            jObject["Reference"] = Reference;
            return jObject;
        }
    }
}

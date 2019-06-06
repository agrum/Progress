using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class UnitStat : Base
    {
        public Skill.UnitStat.EType Stat { get; private set; }
        public Skill.UnitStat.EInputType InputType { get; private set; }
        public MetricReference Reference { get; private set; }

        public UnitStat(
            Id id_,
            Skill.UnitStat.EType stat_,
            Skill.UnitStat.EInputType inputType_,
            MetricReference reference_)
            : base(id_)
        {
            Stat = stat_;
            InputType = inputType_;
            Reference = reference_;
        }

        public UnitStat(
            JSONNode jNode_)
            : base(jNode_)
        {
            Reference = jNode_["reference"];
            Stat = (Skill.UnitStat.EType)Enum.Parse(typeof(Skill.UnitStat.EType), jNode_["stat"]);
            InputType = (Skill.UnitStat.EInputType)Enum.Parse(typeof(Skill.UnitStat.EInputType), jNode_["input"]);
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Stat"] = Stat.ToString("G");
            jObject["InputType"] = InputType.ToString("G");
            jObject["Reference"] = Reference;
            return jObject;
        }
    }
}

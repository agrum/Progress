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
        public SkillMetricReference Reference { get; private set; }

        public UnitStat(
            string name_,
            Skill.UnitStat.EType stat_,
            Skill.UnitStat.EInputType inputType_,
            SkillMetricReference reference_,
            ESubject from_ = ESubject.Trigger,
            ESubject to_ = ESubject.Target)
            : base(name_, from_, to_)
        {
            Stat = stat_;
            InputType = inputType_;
            Reference = reference_;
        }

        public UnitStat(
            JSONNode jNode_)
            : base(jNode_["name"], (ESubject)Enum.Parse(typeof(ESubject), jNode_["from"]), (ESubject)Enum.Parse(typeof(ESubject), jNode_["to"]))
        {
            Reference = jNode_["reference"];
            Stat = (Skill.UnitStat.EType)Enum.Parse(typeof(Skill.UnitStat.EType), jNode_["stat"]);
            InputType = (Skill.UnitStat.EInputType)Enum.Parse(typeof(Skill.UnitStat.EInputType), jNode_["input"]);
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["stat"] = Stat.ToString("G");
            jObject["inputType"] = InputType.ToString("G");
            jObject["reference"] = Reference;
            return jObject;
        }
    }
}

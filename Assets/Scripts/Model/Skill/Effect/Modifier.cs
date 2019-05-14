using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Modifier : Base
    {
        public enum EAction
        {
            Apply,
            Remove,
            Purge,
        }

        public EAction Action { get; private set; }
        public SkillMetricReference Amount { get; private set; }
        public int ModifierId { get; private set; }

        public Modifier(
            string name_,
            EAction action_,
            SkillMetricReference amount_,
            int modifierId_,
            ESubject from_ = ESubject.Trigger,
            ESubject to_ = ESubject.Target)
            : base(name_, from_, to_)
        {
            Action = action_;
            Amount = amount_;
            ModifierId = modifierId_;
        }

        public Modifier(
            JSONNode jNode_)
            : base(jNode_["name"], (ESubject)Enum.Parse(typeof(ESubject), jNode_["from"]), (ESubject)Enum.Parse(typeof(ESubject), jNode_["to"]))
        {
            Action = (EAction)Enum.Parse(typeof(EAction), jNode_["action"]);
            Amount = jNode_["amount"];
            ModifierId = jNode_["modifierId"].AsInt;
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["action"] = Action.ToString("G");
            jObject["amount"] = Amount;
            jObject["modifierId"] = ModifierId;
            return jObject;
        }
    }
}

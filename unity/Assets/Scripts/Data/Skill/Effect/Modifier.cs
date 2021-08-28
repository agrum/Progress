using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
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
        public MetricReference Amount { get; private set; }
        public NamedHash ModifierName { get; private set; }

        public Modifier(
            Id id_,
            EAction action_,
            MetricReference amount_,
            string modifierName_)
            : base(id_)
        {
            Action = action_;
            Amount = amount_;
            ModifierName = modifierName_;
        }

        public Modifier(
            JSONNode jNode_)
            : base(jNode_)
        {
            Action = Serializer.ReadEnum<EAction>(jNode_["action"]);
            Amount = jNode_["amount"];
            ModifierName = jNode_["modifierName"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["action"] = Action;
            jObject["amount"] = Amount;
            jObject["modifierName"] = ModifierName;
            return jObject;
        }
    }
}

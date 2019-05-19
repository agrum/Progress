using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using Assets.Scripts.Model.UnitAttribute;

namespace Assets.Scripts.Model.Skill
{
    public abstract class Trigger
    {
        protected abstract JSONObject ToJson();

        public static implicit operator Trigger(JSONNode jNode_)
        {
            switch (jNode_["Type"].ToString())
            {
                case "TriggerBegin": return new TriggerBegin();
                case "TriggerEnd": return new TriggerEnd();
                case "TriggerEnteredRadius": return new TriggerEnteredRadius(jNode_);
                case "TriggerLeftRadius": return new TriggerLeftRadius(jNode_);
                case "TriggerAttributeOutgoing": return new TriggerAttributeOutgoing(jNode_);
                case "TriggerAttributeIncoming": return new TriggerAttributeIncoming(jNode_);
                case "TriggerInputMove": return new TriggerInputMove(jNode_);
                case "TriggerModifierApplied": return new TriggerModifierApplied(jNode_);
                case "TriggerModifierRemoved": return new TriggerModifierRemoved(jNode_);
                case "TriggerStackAdded": return new TriggerStackAdded(jNode_);
                case "TriggerStackChanged": return new TriggerStackChanged(jNode_);
                case "TriggerStackRemoved": return new TriggerStackRemoved(jNode_);
                case "TriggerTick": return new TriggerTick(jNode_);
                case "TriggerUnitCreated": return new TriggerUnitCreated(jNode_);
                case "TriggerUnitDestroyed": return new TriggerUnitDestroyed(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Trigger triggerType_)
        {
            JSONObject jObject = triggerType_.ToJson();
            jObject["Type"] = triggerType_.GetType().ToString();
            return jObject;
        }
    }

    public class TriggerInputSkillDown : Trigger
    {
        public TriggerInputSkillDown()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerInputSkillUp : Trigger
    {
        public TriggerInputSkillUp()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerInputMove : Trigger
    {
        public SkillMetricReference Distance { get; private set; }

        public TriggerInputMove(SkillMetricReference reference_)
        {
            Distance = reference_;
        }

        public TriggerInputMove(JSONNode jNode_)
        {
            Distance = jNode_["distance"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["distance"] = Distance;
            return jObject;
        }
    }

    public class TriggerBegin : Trigger
    {
        public TriggerBegin()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerTick : Trigger
    {
        public SkillMetricReference Period { get; private set; }

        public TriggerTick(SkillMetricReference reference_)
        {
            Period = reference_;
        }

        public TriggerTick(JSONNode jNode_)
        {
            Period = jNode_["period"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["period"] = Period;
            return jObject;
        }
    }

    public class TriggerEnd : Trigger
    {
        public TriggerEnd()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerStackAdded : Trigger
    {
        public SkillMetricReference Threshold { get; private set; }

        public TriggerStackAdded(SkillMetricReference reference_)
        {
            Threshold = reference_;
        }

        public TriggerStackAdded(JSONNode jNode_)
        {
            Threshold = jNode_["threshold"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["threshold"] = Threshold;
            return jObject;
        }
    }

    public class TriggerStackRemoved : Trigger
    {
        public SkillMetricReference Threshold { get; private set; }

        public TriggerStackRemoved(SkillMetricReference reference_)
        {
            Threshold = reference_;
        }

        public TriggerStackRemoved(JSONNode jNode_)
        {
            Threshold = jNode_["threshold"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["threshold"] = Threshold;
            return jObject;
        }
    }

    public class TriggerStackChanged : Trigger
    {
        public SkillMetricReference Threshold { get; private set; }

        public TriggerStackChanged(SkillMetricReference reference_)
        {
            Threshold = reference_;
        }

        public TriggerStackChanged(JSONNode jNode_)
        {
            Threshold = jNode_["threshold"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["threshold"] = Threshold;
            return jObject;
        }
    }

    public class TriggerUnitCreated : Trigger
    {
        public string Id { get; private set; }

        public TriggerUnitCreated(string id_)
        {
            Id = id_;
        }

        public TriggerUnitCreated(JSONNode jNode_)
        {
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerUnitDestroyed : Trigger
    {
        public string Id { get; private set; }

        public TriggerUnitDestroyed(string id_)
        {
            Id = id_;
        }

        public TriggerUnitDestroyed(JSONNode jNode_)
        {
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerModifierApplied : Trigger
    {
        public string Id { get; private set; }

        public TriggerModifierApplied(string id_)
        {
            Id = id_;
        }

        public TriggerModifierApplied(JSONNode jNode_)
        {
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerModifierRemoved : Trigger
    {
        public string Id { get; private set; }

        public TriggerModifierRemoved(string id_)
        {
            Id = id_;
        }

        public TriggerModifierRemoved(JSONNode jNode_)
        {
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerEnteredRadius : Trigger
    {
        public SkillMetricReference Radius { get; private set; }

        public TriggerEnteredRadius(SkillMetricReference reference_)
        {
            Radius = reference_;
        }

        public TriggerEnteredRadius(JSONNode jNode_)
        {
            Radius = jNode_["radius"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["radius"] = Radius;
            return jObject;
        }
    }

    public class TriggerLeftRadius : Trigger
    {
        public SkillMetricReference Radius { get; private set; }

        public TriggerLeftRadius(SkillMetricReference reference_)
        {
            Radius = reference_;
        }

        public TriggerLeftRadius(JSONNode jNode_)
        {
            Radius = jNode_["radius"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["radius"] = Radius;
            return jObject;
        }
    }

    public class TriggerAttributeOutgoing : Trigger
    {
        public EUnitAttribute Attribute { get; private set; }

        public TriggerAttributeOutgoing(EUnitAttribute attribute_)
        {
            Attribute = attribute_;
        }

        public TriggerAttributeOutgoing(JSONNode jNode_)
        {
            Attribute = (EUnitAttribute)jNode_["attribute"].AsInt;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["attribute"] = (int)Attribute;
            return jObject;
        }
    }

    public class TriggerAttributeIncoming : Trigger
    {
        public EUnitAttribute Attribute { get; private set; }

        public TriggerAttributeIncoming(EUnitAttribute attribute_)
        {
            Attribute = attribute_;
        }

        public TriggerAttributeIncoming(JSONNode jNode_)
        {
            Attribute = (EUnitAttribute)jNode_["attribute"].AsInt;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["attribute"] = (int)Attribute;
            return jObject;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using Assets.Scripts.Model.UnitAttribute;

namespace Assets.Scripts.Model.Skill.Trigger
{
    public abstract class Base
    {
        protected abstract JSONObject ToJson();

        public static implicit operator Base(JSONNode jNode_)
        {
            switch (jNode_["Type"].ToString())
            {
                case "TriggerBegin": return new Begin();
                case "TriggerEnd": return new End();
                case "TriggerEnteredRadius": return new EnteredRadius(jNode_);
                case "TriggerLeftRadius": return new LeftRadius(jNode_);
                case "TriggerAttributeOutgoing": return new AttributeOutgoing(jNode_);
                case "TriggerAttributeIncoming": return new AttributeIncoming(jNode_);
                case "TriggerInputMove": return new InputMove(jNode_);
                case "TriggerModifierApplied": return new ModifierApplied(jNode_);
                case "TriggerModifierRemoved": return new ModifierRemoved(jNode_);
                case "TriggerStackAdded": return new StackAdded(jNode_);
                case "TriggerStackChanged": return new StackChanged(jNode_);
                case "TriggerStackRemoved": return new StackRemoved(jNode_);
                case "TriggerTick": return new Tick(jNode_);
                case "TriggerUnitCreated": return new UnitCreated(jNode_);
                case "TriggerUnitDestroyed": return new UnitDestroyed(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Base triggerType_)
        {
            JSONObject jObject = triggerType_.ToJson();
            jObject["Type"] = triggerType_.GetType().ToString();
            return jObject;
        }
    }
}

namespace Assets.Scripts.Model.Skill.Trigger
{
    public class InputSkillDown : Base
    {
        public InputSkillDown()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class InputSkillUp : Base
    {
        public InputSkillUp()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class InputMove : Base
    {
        public SkillMetricReference Distance { get; private set; }

        public InputMove(SkillMetricReference reference_)
        {
            Distance = reference_;
        }

        public InputMove(JSONNode jNode_)
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

    public class Begin : Base
    {
        public Begin()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class Tick : Base
    {
        public SkillMetricReference Period { get; private set; }

        public Tick(SkillMetricReference reference_)
        {
            Period = reference_;
        }

        public Tick(JSONNode jNode_)
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

    public class End : Base
    {
        public End()
        {
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class StackAdded : Base
    {
        public SkillMetricReference Threshold { get; private set; }

        public StackAdded(SkillMetricReference reference_)
        {
            Threshold = reference_;
        }

        public StackAdded(JSONNode jNode_)
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

    public class StackRemoved : Base
    {
        public SkillMetricReference Threshold { get; private set; }

        public StackRemoved(SkillMetricReference reference_)
        {
            Threshold = reference_;
        }

        public StackRemoved(JSONNode jNode_)
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

    public class StackChanged : Base
    {
        public SkillMetricReference Threshold { get; private set; }

        public StackChanged(SkillMetricReference reference_)
        {
            Threshold = reference_;
        }

        public StackChanged(JSONNode jNode_)
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

    public class UnitCreated : Base
    {
        public string Id { get; private set; }

        public UnitCreated(string id_)
        {
            Id = id_;
        }

        public UnitCreated(JSONNode jNode_)
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

    public class UnitDestroyed : Base
    {
        public string Id { get; private set; }

        public UnitDestroyed(string id_)
        {
            Id = id_;
        }

        public UnitDestroyed(JSONNode jNode_)
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

    public class ModifierApplied : Base
    {
        public string Id { get; private set; }

        public ModifierApplied(string id_)
        {
            Id = id_;
        }

        public ModifierApplied(JSONNode jNode_)
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

    public class ModifierRemoved : Base
    {
        public string Id { get; private set; }

        public ModifierRemoved(string id_)
        {
            Id = id_;
        }

        public ModifierRemoved(JSONNode jNode_)
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

    public class EnteredRadius : Base
    {
        public SkillMetricReference Radius { get; private set; }

        public EnteredRadius(SkillMetricReference reference_)
        {
            Radius = reference_;
        }

        public EnteredRadius(JSONNode jNode_)
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

    public class LeftRadius : Base
    {
        public SkillMetricReference Radius { get; private set; }

        public LeftRadius(SkillMetricReference reference_)
        {
            Radius = reference_;
        }

        public LeftRadius(JSONNode jNode_)
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

    public class AttributeOutgoing : Base
    {
        public EUnitAttribute Attribute { get; private set; }

        public AttributeOutgoing(EUnitAttribute attribute_)
        {
            Attribute = attribute_;
        }

        public AttributeOutgoing(JSONNode jNode_)
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

    public class AttributeIncoming : Base
    {
        public EUnitAttribute Attribute { get; private set; }

        public AttributeIncoming(EUnitAttribute attribute_)
        {
            Attribute = attribute_;
        }

        public AttributeIncoming(JSONNode jNode_)
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

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
        public enum EType
        {
            InputSkillDown,
            InputSkillUp,
            InputMove,
            Begin,
            Tick,
            End,
            StackAdded,
            StackRemoved,
            StackChanged,
            UnitCreated,
            UnitDestroyed,
            ModifierApplied,
            ModifierRemoved,
            EnteredRadius,
            LeftRadius,
            HitDealt,
            HitReceived,
            Custom
        }

        public abstract EType Type();

        protected abstract JSONObject ToJson();

        public static implicit operator Trigger(JSONNode jNode_)
        {
            EType type;
            if (!Enum.TryParse(jNode_["type"], true, out type))
            {
                throw new InvalidOperationException();
            }
            switch (type)
            {
                case EType.Begin: return new TriggerBegin();
                case EType.End: return new TriggerEnd();
                case EType.EnteredRadius: return new TriggerEnteredRadius(jNode_);
                case EType.HitDealt: return new TriggerOtherAttributeImpacted(jNode_);
                case EType.HitReceived: return new TriggerSelfAttributeImpacted(jNode_);
                case EType.InputMove: return new TriggerInputMove(jNode_);
                case EType.LeftRadius: return new TriggerLeftRadius(jNode_);
                case EType.ModifierApplied: return new TriggerModifierApplied(jNode_);
                case EType.ModifierRemoved: return new TriggerModifierRemoved(jNode_);
                case EType.StackAdded: return new TriggerStackAdded(jNode_);
                case EType.StackChanged: return new TriggerStackChanged(jNode_);
                case EType.StackRemoved: return new TriggerStackRemoved(jNode_);
                case EType.Tick: return new TriggerTick(jNode_);
                case EType.UnitCreated: return new TriggerUnitCreated(jNode_);
                case EType.UnitDestroyed: return new TriggerUnitDestroyed(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Trigger triggerType_)
        {
            JSONObject jObject = triggerType_.ToJson();
            jObject["type"] = triggerType_.Type().ToString("G");
            return jObject;
        }
    }

    public class TriggerInputSkillDown : Trigger
    {
        public TriggerInputSkillDown()
        {
        }

        public override EType Type()
        {
            return EType.InputSkillDown;
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

        public override EType Type()
        {
            return EType.InputSkillUp;
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

        public override EType Type()
        {
            return EType.InputMove;
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

        public override EType Type()
        {
            return EType.Begin;
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

        public override EType Type()
        {
            return EType.Tick;
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

        public override EType Type()
        {
            return EType.End;
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

        public override EType Type()
        {
            return EType.StackAdded;
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

        public override EType Type()
        {
            return EType.StackRemoved;
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

        public override EType Type()
        {
            return EType.StackChanged;
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

        public override EType Type()
        {
            return EType.UnitCreated;
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

        public override EType Type()
        {
            return EType.UnitDestroyed;
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

        public override EType Type()
        {
            return EType.ModifierApplied;
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

        public override EType Type()
        {
            return EType.ModifierRemoved;
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

        public override EType Type()
        {
            return EType.EnteredRadius;
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

        public override EType Type()
        {
            return EType.LeftRadius;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["radius"] = Radius;
            return jObject;
        }
    }

    public class TriggerOtherAttributeImpacted : Trigger
    {
        public EUnitAttribute Attribute { get; private set; }

        public TriggerOtherAttributeImpacted(EUnitAttribute attribute_)
        {
            Attribute = attribute_;
        }

        public TriggerOtherAttributeImpacted(JSONNode jNode_)
        {
            Attribute = (EUnitAttribute)jNode_["attribute"].AsInt;
        }

        public override EType Type()
        {
            return EType.HitDealt;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["attribute"] = (int)Attribute;
            return jObject;
        }
    }

    public class TriggerSelfAttributeImpacted : Trigger
    {
        public EUnitAttribute Attribute { get; private set; }

        public TriggerSelfAttributeImpacted(EUnitAttribute attribute_)
        {
            Attribute = attribute_;
        }

        public TriggerSelfAttributeImpacted(JSONNode jNode_)
        {
            Attribute = (EUnitAttribute)jNode_["attribute"].AsInt;
        }

        public override EType Type()
        {
            return EType.HitReceived;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["attribute"] = (int)Attribute;
            return jObject;
        }
    }
}

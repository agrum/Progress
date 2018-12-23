using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using Assets.Scripts.Model.UnitAttribute;

namespace Assets.Scripts.Model.Skill
{
    public abstract class TriggerType
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

        public EType Type { get; protected set; }

        protected abstract JSONObject ToJson();

        public static implicit operator TriggerType(JSONNode jNode_)
        {
            switch ((EType)jNode_["type"].AsInt)
            {
                case EType.Begin: return new TriggerBegin(jNode_);
                case EType.End: return new TriggerEnd(jNode_);
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

        public static implicit operator JSONNode(TriggerType triggerType_)
        {
            JSONObject jObject = triggerType_.ToJson();
            jObject["type"] = (int)triggerType_.Type;
            return jObject;
        }
    }

    public class TriggerInputSkillDown : TriggerType
    {
        public TriggerInputSkillDown(JSONNode jNode_)
        {
            Type = EType.InputSkillDown;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerInputSkillUp : TriggerType
    {
        public TriggerInputSkillUp(JSONNode jNode_)
        {
            Type = EType.InputSkillUp;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerInputMove : TriggerType
    {
        public double Distance { get; private set; }

        public TriggerInputMove(JSONNode jNode_)
        {
            Type = EType.InputMove;
            Distance = jNode_["distance"].AsDouble;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["distance"] = Distance;
            return jObject;
        }
    }

    public class TriggerBegin : TriggerType
    {
        public TriggerBegin(JSONNode jNode_)
        {
            Type = EType.Begin;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerTick : TriggerType
    {
        public double Period { get; private set; }

        public TriggerTick(JSONNode jNode_)
        {
            Type = EType.Tick;
            Period = jNode_["period"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["period"] = Period;
            return jObject;
        }
    }

    public class TriggerEnd : TriggerType
    {
        public TriggerEnd(JSONNode jNode_)
        {
            Type = EType.End;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            return jObject;
        }
    }

    public class TriggerStackAdded : TriggerType
    {
        public double Threshold { get; private set; }

        public TriggerStackAdded(JSONNode jNode_)
        {
            Type = EType.StackAdded;
            Threshold = jNode_["threshold"].AsDouble;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["threshold"] = Threshold;
            return jObject;
        }
    }

    public class TriggerStackRemoved : TriggerType
    {
        public double Threshold { get; private set; }

        public TriggerStackRemoved(JSONNode jNode_)
        {
            Type = EType.StackRemoved;
            Threshold = jNode_["threshold"].AsDouble;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["threshold"] = Threshold;
            return jObject;
        }
    }

    public class TriggerStackChanged : TriggerType
    {
        public double Threshold { get; private set; }

        public TriggerStackChanged(JSONNode jNode_)
        {
            Type = EType.StackChanged;
            Threshold = jNode_["threshold"].AsDouble;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["threshold"] = Threshold;
            return jObject;
        }
    }

    public class TriggerUnitCreated : TriggerType
    {
        public String Id { get; private set; }

        public TriggerUnitCreated(JSONNode jNode_)
        {
            Type = EType.UnitCreated;
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerUnitDestroyed : TriggerType
    {
        public String Id { get; private set; }

        public TriggerUnitDestroyed(JSONNode jNode_)
        {
            Type = EType.UnitDestroyed;
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerModifierApplied : TriggerType
    {
        public String Id { get; private set; }

        public TriggerModifierApplied(JSONNode jNode_)
        {
            Type = EType.ModifierApplied;
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerModifierRemoved : TriggerType
    {
        public String Id { get; private set; }

        public TriggerModifierRemoved(JSONNode jNode_)
        {
            Type = EType.ModifierRemoved;
            Id = jNode_["id"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["id"] = Id;
            return jObject;
        }
    }

    public class TriggerEnteredRadius : TriggerType
    {
        public double Radius { get; private set; }

        public TriggerEnteredRadius(JSONNode jNode_)
        {
            Type = EType.EnteredRadius;
            Radius = jNode_["radius"].AsDouble;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["radius"] = Radius;
            return jObject;
        }
    }

    public class TriggerLeftRadius : TriggerType
    {
        public double Radius { get; private set; }

        public TriggerLeftRadius(JSONNode jNode_)
        {
            Type = EType.LeftRadius;
            Radius = jNode_["radius"].AsDouble;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["radius"] = Radius;
            return jObject;
        }
    }

    public class TriggerOtherAttributeImpacted : TriggerType
    {
        public EUnitAttribute Attribute { get; private set; }

        public TriggerOtherAttributeImpacted(JSONNode jNode_)
        {
            Type = EType.HitDealt;
            Attribute = (EUnitAttribute)jNode_["attribute"].AsInt;
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["attribute"] = (int)Attribute;
            return jObject;
        }
    }

    public class TriggerSelfAttributeImpacted : TriggerType
    {
        public EUnitAttribute Attribute { get; private set; }

        public TriggerSelfAttributeImpacted(JSONNode jNode_)
        {
            Type = EType.HitDealt;
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

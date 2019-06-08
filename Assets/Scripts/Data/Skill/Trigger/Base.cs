using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Trigger
{
    public abstract class Base
    {
        protected abstract JSONObject ToJson();

        public static implicit operator Base(JSONNode jNode_)
        {
            switch (jNode_["Type"].ToString())
            {
                case "Begin": return new Begin();
                case "End": return new End();
                case "EnteredRadius": return new EnteredRadius(jNode_);
                case "LeftRadius": return new LeftRadius(jNode_);
                case "UnitChange": return new UnitChange(jNode_);
                case "InputMove": return new InputMove(jNode_);
                case "ModifierApplied": return new ModifierApplied(jNode_);
                case "ModifierRemoved": return new ModifierRemoved(jNode_);
                case "StackAdded": return new StackAdded(jNode_);
                case "StackChanged": return new StackChanged(jNode_);
                case "StackRemoved": return new StackRemoved(jNode_);
                case "Tick": return new Tick(jNode_);
                case "UnitCreated": return new UnitCreated(jNode_);
                case "UnitDestroyed": return new UnitDestroyed(jNode_);
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

namespace Assets.Scripts.Data.Skill.Trigger
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
        public MetricReference Distance { get; private set; }

        public InputMove(MetricReference reference_)
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
        public MetricReference Period { get; private set; }

        public Tick(MetricReference reference_)
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
        public MetricReference Threshold { get; private set; }

        public StackAdded(MetricReference reference_)
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
        public MetricReference Threshold { get; private set; }

        public StackRemoved(MetricReference reference_)
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
        public MetricReference Threshold { get; private set; }

        public StackChanged(MetricReference reference_)
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
        public MetricReference Radius { get; private set; }

        public EnteredRadius(MetricReference reference_)
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
        public MetricReference Radius { get; private set; }

        public LeftRadius(MetricReference reference_)
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

    public class GaugeOutput : Base
    {
        public NamedHash Stat { get; private set; }

        public GaugeOutput(NamedHash stat_)
        {
            Stat = stat_;
        }

        public GaugeOutput(JSONNode jNode_)
        {
            Stat = jNode_["Stat"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["Stat"] = Stat;
            return jObject;
        }
    }

    public class UnitChange : Base
    {
        public enum EDirection
        {
            Output,
            Input,
            Both
        }

        public enum EStatType
        {
            Stat,
            Gauge,
            Cooldown
        }

        public EDirection Direction { get; private set; }
        public EStatType StatType { get; private set; }
        public NamedHash Name { get; private set; }

        public UnitChange(EDirection direction_, EStatType statType_, NamedHash name_)
        {
            Direction = direction_;
            StatType = statType_;
            Name = name_;
        }

        public UnitChange(JSONNode jNode_)
        {
            Direction = Serializer.ReadEnum<EDirection>(jNode_["Direction"]);
            StatType = Serializer.ReadEnum<EStatType>(jNode_["StatType"]);
            Name = jNode_["Name"];
        }

        protected override JSONObject ToJson()
        {
            var jObject = new JSONObject();
            jObject["Stat"] = Direction;
            jObject["Stat"] = StatType;
            jObject["Stat"] = Name;
            return jObject;
        }
    }
}

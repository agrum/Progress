using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Base
    {
        public struct Id
        {
            public NamedHash Name { get; private set; }
            public ESubject From { get; private set; }
            public ESubject To { get; private set; }

            public Id(string name_, ESubject from_ = ESubject.Trigger, ESubject to_ = ESubject.Target)
            {
                Name = name_;
                From = from_;
                To = to_;
            }

            public static implicit operator Id(string nameHash_)
            {
                return new Id(nameHash_);
            }
        }

        public enum EDirection
        {
            SourceAim,
            TargetAim,
            TriggerAim,
            SourceToTarget,
            SourceToTrigger,
            TargetToSource,
            TargetToTrigger,
            TriggerToSource,
            TriggerToTarget,
        }

        public NamedHash Name { get; private set; }
        public ESubject From { get; private set; }
        public ESubject To { get; private set; }

        virtual public JSONObject ToJson()
        {
            return null;
        }

        protected Base(JSONNode jNode_)
        {
            Name = jNode_["name"];
            From = Serializer.ReadEnum<ESubject>(jNode_["from"]);
            To = Serializer.ReadEnum<ESubject>(jNode_["to"]);
        }

        protected Base(Id id_)
        {
            Name = id_.Name;
            From = id_.From;
            To = id_.To;
        }

        protected Base(string name_, ESubject from_, ESubject to_)
        {
            Name = name_;
            From = from_;
            To = to_;
        }

        public static implicit operator Base(JSONNode jNode_)
        {
            switch (jNode_["type"].ToString())
            {
                case "Area": return new Area(jNode_);
                case "Converter": return new Converter(jNode_);
                case "Modifier": return new Modifier(jNode_);
                case "Physics": return new Physics(jNode_);
                case "Gauge": return new Gauge(jNode_);
                case "Stat": return new Stat(jNode_);
                case "Cooldown": return new Cooldown(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Base object_)
        {
            JSONObject jObject = object_.ToJson();
            jObject["type"] = object_.GetType().Name;
            jObject["name"] = object_.Name;
            jObject["from"] = object_.From;
            jObject["to"] = object_.To;
            return jObject;
        }
    }
}

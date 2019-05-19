using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class Base
    {
        public struct Id
        {
            public NamedHash NamedHash { get; private set; }
            public ESubject From { get; private set; }
            public ESubject To { get; private set; }

            public Id(string namedHash_, ESubject from_ = ESubject.Trigger, ESubject to_ = ESubject.Target)
            {
                NamedHash = namedHash_;
                From = from_;
                To = to_;
            }

            public static implicit operator Id(string nameHash_)
            {
                return nameHash_;
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

        public NamedHash NamedHash { get; private set; }
        public ESubject From { get; private set; }
        public ESubject To { get; private set; }

        virtual public JSONObject ToJson()
        {
            return null;
        }

        protected Base(JSONNode jNode_)
        {
            NamedHash = jNode_["Name"];
            From = (ESubject)Enum.Parse(typeof(ESubject), jNode_["From"]);
            To = (ESubject)Enum.Parse(typeof(ESubject), jNode_["To"]);
        }

        protected Base(Id id_)
        {
            NamedHash = id_.NamedHash;
            From = id_.From;
            To = id_.To;
        }

        protected Base(string namedHash_, ESubject from_, ESubject to_)
        {
            NamedHash = namedHash_;
            From = from_;
            To = to_;
        }

        public static implicit operator Base(JSONNode jNode_)
        {
            switch (jNode_["Type"].ToString())
            {
                case "Area": return new UnitStat(jNode_);
                case "Converter": return new Modifier(jNode_);
                case "Modifier": return new Modifier(jNode_);
                case "Physics": return new Modifier(jNode_);
                case "UnitGauge": return new UnitGauge(jNode_);
                case "UnitStat": return new UnitGauge(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Base object_)
        {
            JSONObject jObject = object_.ToJson();
            jObject["Type"] = object_.GetType().Name;
            jObject["NamedHash"] = object_.NamedHash;
            jObject["From"] = object_.From.ToString("G");
            jObject["To"] = object_.To.ToString("G");
            return jObject;
        }
    }
}

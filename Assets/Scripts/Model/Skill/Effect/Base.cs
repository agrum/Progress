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
        public enum EType
        {
            UnitStat,
            UnitGauge,
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public ESubject From { get; private set; }
        public ESubject To { get; private set; }

        virtual public EType Type()
        {
            return EType.UnitStat;
        }

        virtual public JSONObject ToJson()
        {
            return null;
        }

        protected Base(string id_, string name_, ESubject from_, ESubject to_)
        {
            Id = id_;
            Name = name_;
            From = from_;
            To = to_;
        }

        public static implicit operator Base(JSONNode jNode_)
        {
            EType type;
            if (!Enum.TryParse(jNode_["effect"], true, out type))
            {
                throw new InvalidOperationException();
            }
            switch (type)
            {
                case EType.UnitStat: return new UnitStat(jNode_);
                case EType.UnitGauge: return new EffectUnitGauge(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Base object_)
        {
            JSONObject jObject = object_.ToJson();
            jObject["nameId"] = object_.Id;
            jObject["name"] = object_.Name;
            jObject["effect"] = object_.Type().ToString("G");
            jObject["from"] = object_.Id;
            jObject["to"] = object_.Name;
            return jObject;
        }
    }
}

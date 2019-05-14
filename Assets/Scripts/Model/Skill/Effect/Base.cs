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
        public int Hash { get; private set; }
        public string Name { get; private set; }
        public ESubject From { get; private set; }
        public ESubject To { get; private set; }

        virtual public JSONObject ToJson()
        {
            return null;
        }

        protected Base(string name_, ESubject from_, ESubject to_)
        {
            Hash = (GetType().Name + "." + name_).GetHashCode();
            Name = name_;
            From = from_;
            To = to_;
        }

        public static implicit operator Base(JSONNode jNode_)
        {
            switch (jNode_["type"].ToString())
            {
                case "UnitStat": return new UnitStat(jNode_);
                case "UnitGauge": return new UnitGauge(jNode_);
                case "Modifier": return new Modifier(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Base object_)
        {
            JSONObject jObject = object_.ToJson();
            jObject["type"] = object_.GetType().Name;
            jObject["hash"] = object_.Hash;
            jObject["name"] = object_.Name;
            jObject["from"] = object_.From.ToString("G");
            jObject["to"] = object_.To.ToString("G");
            return jObject;
        }
    }
}

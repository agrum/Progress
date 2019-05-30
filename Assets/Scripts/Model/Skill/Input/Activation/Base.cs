using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Input.Activation
{
    public abstract class Base
    {
        abstract public JSONObject ToJson();

        public static implicit operator Base(JSONNode jNode_)
        {
            switch (jNode_["Type"].ToString())
            {
                case "Beam": return new Beam(jNode_);
                case "Cone": return new Cone(jNode_);
                case "Drop": return new Cone(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Base object_)
        {
            JSONObject jObject = object_.ToJson();
            jObject["Type"] = object_.GetType().Name;
            return jObject;
        }
    }
}

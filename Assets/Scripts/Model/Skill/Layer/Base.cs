using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer
{
    public abstract class Base
    {
        public Visual Visual { get; private set; }

        protected Base(Visual visual_)
        {
            Visual = visual_;
        }

        protected Base(JSONNode jNode_)
        {
            Visual = jNode_["Visual"];
        }

        abstract public JSONObject ToJson();

        public static implicit operator Base(JSONNode jNode_)
        {
            switch (jNode_["Type"].ToString())
            {
                case "Active": return new Active(jNode_);
                case "Passive": return new Passive(jNode_);
                default: return null;
            }
        }

        public static implicit operator JSONNode(Base object_)
        {
            JSONObject jObject = object_.ToJson();
            jObject["Type"] = object_.GetType().Name;
            jObject["Visual"] = object_.Visual;
            return jObject;
        }
    }
}

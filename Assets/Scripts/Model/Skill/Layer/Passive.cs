using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer
{
    public class Passive : Base
    {
        public Passive(
            Visual visual_)
            : base(visual_)
        {
        }

        public Passive(JSONNode jNode_)
            : base(jNode_)
        {
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            return jObject;
        }
    }
}

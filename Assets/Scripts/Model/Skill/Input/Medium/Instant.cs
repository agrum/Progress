using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Input.Medium
{
    public class Instant : Base
    {
        public Instant()
        {
        }

        public Instant(JSONNode jNode_)
        {
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            return jObject;
        }
    }
}

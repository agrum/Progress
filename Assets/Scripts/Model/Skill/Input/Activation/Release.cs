﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Input.Activation
{
    public class Release : Base
    {
        public Release()
        {
        }

        public Release(JSONNode jNode_)
        {
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            return jObject;
        }
    }
}

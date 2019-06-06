﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer.Medium
{
    public class Channel : Base
    {
        public MetricReference Duration { get; private set; }

        public Channel(MetricReference duration_)
        {
            Duration = duration_;
        }

        public Channel(JSONNode jNode_)
        {
            Duration = jNode_["Duration"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Duration"] = Duration;
            return jObject;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Layer.Control
{
    public class Cone : Base
    {
        public MetricReference Radius { get; private set; }
        public MetricReference Degrees { get; private set; }

        public Cone(MetricReference radius_, MetricReference degrees_)
        {
            Radius = radius_;
            Degrees = degrees_;
        }

        public Cone(JSONNode jNode_)
        {
            Radius = jNode_["Radius"];
            Degrees = jNode_["Degrees"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Radius"] = Radius;
            jObject["Degrees"] = Degrees;
            return jObject;
        }
    }
}

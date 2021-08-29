﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Delay : Base
    {
        public MetricReference Duration { get; private set; }
        public Base Effect { get; private set; }

        public Delay(
            Id id_,
            MetricReference duration_,
            Base effect_)
            : base(id_)
        {
            Duration = duration_;
            Effect = effect_;
        }

        public Delay(
            JSONNode jNode_)
            : base(jNode_)
        {
            Duration = jNode_["duration"];
            Effect = jNode_["effect"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["duration"] = Duration;
            jObject["effect"] = Effect;
            return jObject;
        }
    }
}
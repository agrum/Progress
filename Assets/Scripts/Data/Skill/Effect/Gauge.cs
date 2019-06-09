﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Gauge : Base
    {
        public enum ETarget
        {
            Capacity,
            CapacityAndProportionalValue,
            CapacityAndValue
        }
        
        public NamedHash Hash { get; private set; }
        public ETarget Target { get; private set; }
        public Data.Skill.Unit.Stat.ECategory Category { get; private set; }
        public Data.Skill.Unit.Gauge.EInputType InputType { get; private set; }
        public MetricReference Reference { get; private set; }

        public Gauge(
            Id id_,
            NamedHash hash_,
            ETarget target_,
            Data.Skill.Unit.Stat.ECategory category_,
            Data.Skill.Unit.Gauge.EInputType inputType_,
            MetricReference reference_)
            : base(id_)
        {
            Hash = hash_;
            Target = target_;
            Category = category_;
            InputType = inputType_;
            Reference = reference_;
        }

        public Gauge(
            JSONNode jNode_)
            : base(jNode_)
        {
            Hash = jNode_["Hash"];
            Target = Serializer.ReadEnum<ETarget>(jNode_["Target"]);
            Category = Serializer.ReadEnum<Data.Skill.Unit.Stat.ECategory>(jNode_["Category"]);
            InputType = Serializer.ReadEnum<Data.Skill.Unit.Gauge.EInputType>(jNode_["Target"]);
            Reference = jNode_["Reference"];
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["Hash"] = Hash;
            jObject["Target"] = Target;
            jObject["Category"] = Category;
            jObject["InputType"] = InputType;
            jObject["Reference"] = Reference;
            return jObject;
        }
    }
}

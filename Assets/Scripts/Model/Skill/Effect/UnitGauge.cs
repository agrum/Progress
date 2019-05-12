﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model.Skill.Effect
{
    public class UnitGauge : Base
    {
        public enum ETarget
        {
            Value,
            Capacity,
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public Skill.UnitGauge.EType Gauge { get; private set; }
        public ETarget Target { get; private set; }
        public Skill.UnitGauge.EInputType InputType { get; private set; }
        public SkillMetricReference Reference { get; private set; }

        public UnitGauge(
            string id_, 
            string name_,
            Skill.UnitGauge.EType gauge_,
            ETarget target_,
            Skill.UnitGauge.EInputType inputType_,
            SkillMetricReference reference_,
            ESubject from_,
            ESubject to_)
            : base(id_, name_, from_, to_)
        {
            Id = id_;
            Name = name_;
            Gauge = gauge_;
            Target = target_;
            InputType = inputType_;
            Reference = reference_;
        }

        public UnitGauge(
            JSONNode jNode_)
            : base(jNode_["nameId"], jNode_["name"], (ESubject) Enum.Parse(typeof(ESubject), jNode_["from"]), (ESubject) Enum.Parse(typeof(ESubject), jNode_["to"]))
        {
            Reference = jNode_["reference"];
            Gauge = (Skill.UnitGauge.EType)Enum.Parse(typeof(Skill.UnitGauge.EType), jNode_["gauge"]);
            Target = (ETarget)Enum.Parse(typeof(ETarget), jNode_["target"]);
            InputType = (Skill.UnitGauge.EInputType)Enum.Parse(typeof(Skill.UnitGauge.EInputType), jNode_["input"]);
        }

        public override EType Type()
        {
            return EType.UnitGauge;
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["target"] = Target.ToString("G");
            jObject["gauge"] = Gauge.ToString("G");
            jObject["inputType"] = InputType.ToString("G");
            jObject["reference"] = Reference;
            return jObject;
        }
    }
}

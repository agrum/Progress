﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Data.Skill.Effect
{
    public class Area : Base
    {
        public enum EShape
        {
            Beam,
            Cone,
        }

        public EShape Shape { get; private set; }
        public MetricReference Length { get; private set; }
        public MetricReference Width { get; private set; }
        public MetricReference Offset { get; private set; }
        public MetricReference AffectMaxCount { get; private set; }
        public List<Base> Effects { get; private set; } = new List<Base>();

        public Area(
            Id id_,
            EShape shape_,
            MetricReference length_,
            MetricReference width_,
            MetricReference offset_,
            MetricReference affectMaxCount_,
            params Base[] effects_)
            : base(id_)
        {
            Shape = shape_;
            Length = length_;
            Width = width_;
            Offset = offset_;
            AffectMaxCount = affectMaxCount_;
            Effects = new List<Base>(effects_);
        }

        public Area(JSONNode jNode_)
            : base(jNode_)
        {
            Shape = Serializer.ReadEnum<EShape>(jNode_["shape"]);
            Length = jNode_["length"];
            Width = jNode_["width"];
            Offset = jNode_["offset"];
            AffectMaxCount = jNode_["affectMaxCount"];
            foreach (var effect in jNode_["effects"].AsArray)
                Effects.Add(effect.Value);
        }

        public override JSONObject ToJson()
        {
            JSONObject jObject = new JSONObject();
            jObject["shape"] = Shape;
            jObject["length"] = Length;
            jObject["width"] = Width;
            jObject["offset"] = Offset;
            jObject["affectMaxCount"] = AffectMaxCount;
            JSONArray effects = new JSONArray();
            foreach (var effect in Effects)
                effects.Add(effect);
            jObject["effects"] = effects;
            return jObject;
        }
    }
}
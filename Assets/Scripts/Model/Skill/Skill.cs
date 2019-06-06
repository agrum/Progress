using System;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Skill
{
    public class Skill
    {
        public static Skill Reference;

        public NamedHash Name { get; private set; }
        public List<Metric> Metrics { get; private set; } = new List<Metric>();
        public List<ModifierBehaviour> Passives { get; private set; } = new List<ModifierBehaviour>();
        public List<Layer.Base> Layers { get; private set; } = new List<Layer.Base>();

        public Skill(NamedHash name_, List<Metric> metrics_, List<ModifierBehaviour> passives_, List<Layer.Base> layers_)
        {
            Reference = this;

            Name = name_;
            Metrics = metrics_;
            Passives = passives_;
            Layers = layers_;

            Reference = null;
        }

        public Skill(JSONNode jNode_)
        {
            Name = jNode_["Name"];

            foreach (var condition in jNode_["Metrics"].AsArray)
                Metrics.Add(condition.Value.AsArray);
            foreach (var effect in jNode_["Passives"].AsArray)
                Passives.Add(effect.Value);
            foreach (var effect in jNode_["Layers"].AsArray)
                Layers.Add(effect.Value);
        }

        public static implicit operator Skill(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Skill skill_)
        {
            JSONObject jObject = new JSONObject();

            jObject["Name"] = skill_.Name;
            var conditions = new JSONArray();
            foreach (var entry in skill_.Metrics)
                conditions.Add(entry);
            jObject["Metrics"] = conditions;
            var effects = new JSONArray();
            foreach (var entry in skill_.Passives)
                effects.Add(entry);
            jObject["Passives"] = conditions;
            foreach (var entry in skill_.Layers)
                effects.Add(entry);
            jObject["Layers"] = conditions;

            return jObject;
        }

        public Metric GetMetric(NamedHash name_)
        {
           return Metrics.Find(x => x.Name == name_);
        }
    }
}


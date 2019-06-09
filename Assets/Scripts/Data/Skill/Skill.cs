using System;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.Skill
{
    public class Skill
    {
        public enum ECategory
        {
            Ability,
            Class,
            Kit,
        }

        public static Skill Reference;

        public Guid _Id { get; private set; }
        public ECategory Category { get; private set; }
        public NamedHash Name { get; private set; }
        public string Description { get; private set; }
        public string Details { get; private set; }
        public List<Metric> Metrics { get; private set; } = new List<Metric>();
        public List<ModifierBehaviour> Passives { get; private set; } = new List<ModifierBehaviour>();
        public List<Layer.Base> Layers { get; private set; } = new List<Layer.Base>();
        public Material Material { get; private set; }

        public Skill(ECategory category_, NamedHash name_, string description_, string details_, List<Metric> metrics_, List<ModifierBehaviour> passives_, List<Layer.Base> layers_)
        {
            Reference = this;

            _Id = Guid.NewGuid();
            Category = category_;
            Name = name_;
            Description = description_;
            Details = details_;
            Metrics = metrics_;
            Passives = passives_;
            Layers = layers_;

            switch (Category)
            {
                case ECategory.Ability:
                    Material = App.Resource.Material.AbilityMaterial;
                    break;
                case ECategory.Class:
                    Material = App.Resource.Material.ClassMaterial;
                    break;
                case ECategory.Kit:
                    Material = App.Resource.Material.KitMaterial;
                    break;
            }

            Reference = null;
        }

        public Skill(JSONNode jNode_)
        {
            _Id = new Guid(jNode_["_id"]);
            Category = Serializer.ReadEnum< ECategory>(jNode_["category"]);
            Name = jNode_["name"];
            Description = jNode_["description"];
            Details = jNode_["details"];

            foreach (var condition in jNode_["metrics"].AsArray)
                Metrics.Add(condition.Value.AsArray);
            foreach (var effect in jNode_["passives"].AsArray)
                Passives.Add(effect.Value);
            foreach (var effect in jNode_["layers"].AsArray)
                Layers.Add(effect.Value);
        }

        public static implicit operator Skill(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Skill skill_)
        {
            JSONObject jObject = new JSONObject();

            jObject["_id"] = skill_._Id.ToString();
            jObject["category"] = skill_.Category;
            jObject["name"] = skill_.Name;
            jObject["description"] = skill_.Description;
            jObject["details"] = skill_.Details;
            var conditions = new JSONArray();
            foreach (var entry in skill_.Metrics)
                conditions.Add(entry);
            jObject["metrics"] = conditions;
            var effects = new JSONArray();
            foreach (var entry in skill_.Passives)
                effects.Add(entry);
            jObject["passives"] = conditions;
            foreach (var entry in skill_.Layers)
                effects.Add(entry);
            jObject["layers"] = conditions;

            return jObject;
        }

        public Metric GetMetric(NamedHash name_)
        {
           return Metrics.Find(x => x.Name == name_);
        }
    }
}


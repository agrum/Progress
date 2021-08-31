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

        public string _Id { get; private set; }
        public ECategory Category { get; private set; }
        public NamedHash Name { get; private set; }
        public string Description { get; private set; }
        public string Details { get; private set; }
        public List<Metric> Metrics { get; private set; } = new List<Metric>();
        public Material Material { get; private set; }

        public Skill(ECategory category_, NamedHash name_, string description_, string details_, List<Metric> metrics_)
        {
            Reference = this;

            Category = category_;
            Name = name_;
            Description = description_;
            Details = details_;
            Metrics = metrics_;

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
            _Id = jNode_["_id"].ToString();
            Category = Serializer.ReadEnum< ECategory>(jNode_["category"]);
            Name = jNode_["name"];
            Description = jNode_["description"];
            Details = jNode_["details"];

            foreach (var condition in jNode_["metrics"].AsArray)
                Metrics.Add(condition.Value);
        }

        public static implicit operator Skill(JSONNode jNode_)
        {
            return new Skill(jNode_);
        }

        public static implicit operator JSONNode(Skill skill_)
        {
            JSONObject jObject = new JSONObject();

            if (skill_._Id != null)
            {
                jObject["_id"] = skill_._Id.ToString();
            }
            jObject["category"] = skill_.Category;
            jObject["name"] = skill_.Name;
            jObject["description"] = skill_.Description;
            jObject["details"] = skill_.Details;
            var conditions = new JSONArray();
            foreach (var entry in skill_.Metrics)
                conditions.Add(entry);
            jObject["metrics"] = conditions;

            return jObject;
        }

        public Metric GetMetric(NamedHash name_)
        {
           return Metrics.Find(x => x.Name.Equals(name_));
        }
    }
}


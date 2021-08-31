using System;
using SimpleJSON;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assets.Scripts.Data.Skill
{
    public class Metric
    {
        public enum ETag
        {
            Physical,
            Elemental,
            Chaos,

            Melee,
            Ranged,
            AoE,

            Attack,
            Spell,
            Trigger,
            Aura,

            Duration,
            Cooldown,
            ActionSpeed,
        }

        public string _Id { get; private set; }
        public NamedHash Name { get; private set; }
        public List<ETag> Tags { get; private set; } = new List<ETag>();
        public Numeric Numeric { get; private set; }

        public Metric(NamedHash name_, List<ETag> tags_, Numeric numeric_)
        {
            Name = name_;
            Tags = tags_;
            Numeric = numeric_;

            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            if (!regex.IsMatch(Name.String))
            {
                throw new InvalidOperationException();
            }
        }

        public Metric(JSONObject jObject_)
        {
            _Id = jObject_["_id"].ToString();
            Name = jObject_["name"];
            foreach (var tag in jObject_["tags"].AsArray)
            {
                Tags.Add(Serializer.ReadEnum<ETag>(tag));
            }
            Numeric = jObject_["numeric"];

            Regex regex = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            if (!regex.IsMatch(Name.String))
            {
                throw new InvalidOperationException();
            }
        }

        public static implicit operator Metric(JSONNode jNode_)
        {
            if (jNode_ != null && jNode_.IsObject)
                return new Metric(jNode_.AsObject);
            throw new WestException("Metric's JSON is not an object");
        }

        public static implicit operator JSONNode(Metric object_)
        {
            JSONArray jTags = new JSONArray();
            foreach (var tag in object_.Tags)
            {
                jTags.Add(Serializer.WriteEnum(tag));
            }

            JSONObject jObject = new JSONObject();
            if (object_._Id != null)
            {
                jObject["_id"] = object_._Id.ToString();
            }
            jObject["name"] = object_.Name;
            jObject["tags"] = jTags;
            jObject["numeric"] = object_.Numeric;
            return jObject;
        }
    }
}

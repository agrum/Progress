using System;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Skill
{
	public abstract class Skill
    {
		public enum TypeEnum
		{
			Ability,
			Kit,
			Class,
			None,
		}

		public TypeEnum Type { get; private set; } = TypeEnum.None;
		public JSONNode Json { get; private set; } = null;
        public Material Material { get; private set; } = null;
        public string Uuid { get; private set; } = "";
		public string LowerCaseKey { get; private set; } = "";
		public string UpperCamelCaseKey { get; private set; } = "";

        public SkillMetric Metric(string idName_)
        {
            return metricMap[idName_];
        }

        public IList<SkillMetric> MetrictList { get { return metricList.AsReadOnly(); } }

        private List<SkillMetric> metricList = new List<SkillMetric>();
        private Dictionary<string, SkillMetric> metricMap = new Dictionary<string, SkillMetric>();

        protected Skill(JSONNode json_, TypeEnum type_)
		{
			Type = type_;
			Json = json_;
			Uuid = json_["_id"];
				
			switch (Type)
			{
				case TypeEnum.Ability:
                    Material = App.Resource.Material.AbilityMaterial;
                    LowerCaseKey = "abilities";
					UpperCamelCaseKey = "Abilities";

                    break;
				case TypeEnum.Class:
                    Material = App.Resource.Material.ClassMaterial;
                    LowerCaseKey = "classes";
					UpperCamelCaseKey = "Classes";
                    break;
				case TypeEnum.Kit:
                    Material = App.Resource.Material.KitMaterial;
                    LowerCaseKey = "kits";
					UpperCamelCaseKey = "Kits";
                    break;
				default:
					throw new Exception();

			}

            Json["color"] = ColorUtility.ToHtmlStringRGBA(Material.color);
            Json["typeName"] = UpperCamelCaseKey;

            foreach (var metricNode in Json["metrics2"].AsArray)
            {
                SkillMetric metric = new SkillMetric(metricNode.Value.AsObject);
                metricList.Add(metric);
                metricMap.Add(metric.IdName, metric);
            }
        }
	}

    public class Skill2
    {
        public NamedHash Name { get; private set; }
        public List<Numeric> Numerics { get; private set; } = new List<Numeric>();
        public List<ModifierBehaviour> Passives { get; private set; } = new List<ModifierBehaviour>();
        public List<Layer.Base> Layers { get; private set; } = new List<Layer.Base>();

        public Skill2(NamedHash name_, List<Numeric> numerics_, List<ModifierBehaviour> passives_, List<Layer.Base> layers_)
        {
            Name = name_;
            Numerics = numerics_;
            Passives = passives_;
            Layers = layers_;
        }

        public Skill2(JSONNode jNode_)
        {
            Name = jNode_["Name"];

            foreach (var condition in jNode_["Numerics"].AsArray)
                Numerics.Add(condition.Value.AsArray);
            foreach (var effect in jNode_["Passives"].AsArray)
                Passives.Add(effect.Value);
            foreach (var effect in jNode_["Layers"].AsArray)
                Layers.Add(effect.Value);
        }

        public static implicit operator Skill2(JSONNode jNode_)
        {
            return jNode_;
        }

        public static implicit operator JSONNode(Skill2 skill_)
        {
            JSONObject jObject = new JSONObject();

            jObject["Name"] = skill_.Name;
            var conditions = new JSONArray();
            foreach (var condition in skill_.Numerics)
                conditions.Add(condition);
            jObject["Numerics"] = conditions;
            var effects = new JSONArray();
            foreach (var effect in skill_.Passives)
                effects.Add(effect);
            jObject["Passives"] = conditions;
            foreach (var effect in skill_.Layers)
                effects.Add(effect);
            jObject["Layers"] = conditions;

            return jObject;
        }


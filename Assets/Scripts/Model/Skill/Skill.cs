using System;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
	public class Skill
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

        public SkillMetric Metric(string category_, string name_)
        {
            return metricMap[SkillMetric.Hash(category_, name_)];
        }

        public IList<SkillMetric> MetrictList { get { return metricList.AsReadOnly(); } }

        private List<SkillMetric> metricList = null;
        private Dictionary<string, SkillMetric> metricMap = null;

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

            foreach (var metricNode in Json["metric2"].AsArray)
            {
                SkillMetric metric = new SkillMetric(metricNode.Value.AsObject);
                metricList.Add(metric);
                metricMap.Add(SkillMetric.Hash(metric.Category, metric.Name), metric);
            }
        }
	}
}


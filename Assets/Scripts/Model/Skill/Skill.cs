using System;
using SimpleJSON;
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
        }
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace West
{
	namespace Model
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
						LowerCaseKey = "abilities";
						UpperCamelCaseKey = "Abilities";
						break;
					case TypeEnum.Class:
						LowerCaseKey = "classes";
						UpperCamelCaseKey = "Classes";
						break;
					case TypeEnum.Kit:
						LowerCaseKey = "kits";
						UpperCamelCaseKey = "Kits";
						break;
					default:
						throw new Exception();
				}
			}
		}
	}
}

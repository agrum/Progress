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
		public class Ability : Skill
		{
			public Ability(JSONNode json_) : base(json_, Skill.TypeEnum.Ability)
			{

			}
		}
	}
}

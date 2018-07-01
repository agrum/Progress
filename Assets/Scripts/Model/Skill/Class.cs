using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model
{
	public class Class : Skill
	{
		public Class(JSONNode json_) : base(json_, Skill.TypeEnum.Class)
		{

		}
	}
}

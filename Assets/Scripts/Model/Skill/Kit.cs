using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;

namespace Assets.Scripts.Model
{
	public class Kit : Skill
	{
		public Kit(JSONNode json_) : base(json_, Kit.TypeEnum.Kit)
		{

		}
	}
}

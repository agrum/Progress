using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using BestHTTP;

namespace West
{
	namespace Model
	{
		namespace CloudContent
		{
			public class Constellation : Base
			{
				public JSONNode Json { get; private set; } = null;
				public Model.Constellation Model { get; private set; } = null;

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Server.Request(
					HTTPMethods.Get,
					"constellation/Hexagon36",
					(JSONNode json_) =>
					{
						Json = json_;
						Model = new Model.Constellation(Json);
						onBuilt_();
					}).Send();
				}

				public Constellation(AbilityList abilityList_, ClassList classList_, KitList kitList_)
				{
					dependencyList.Add(abilityList_);
					dependencyList.Add(classList_);
					dependencyList.Add(kitList_);
				}
			}
		}
	}
}

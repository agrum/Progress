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
			public class KitList : Base
			{
				public JSONNode Json { get; private set; } = null;

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Request(
					HTTPMethods.Get,
					"kit",
					(JSONNode json_) =>
					{
						Json = json_;
						onBuilt_();
					}).Send();
				}

				public KitList(GameSettings gameSettings_)
				{
					dependencyList.Add(gameSettings_);
				}
			}
		}
	}
}

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
			public class GameSettings : Base
			{
				public JSONNode Json { get; private set; } = null;

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Request(
					HTTPMethods.Get,
					"gameSettings/Classic",
					(JSONNode json_) =>
					{
						Json = json_;
						onBuilt_();
					}).Send();
				}

				public GameSettings(Session session_)
				{
					dependencyList.Add(session_);
				}
			}
		}
	}
}

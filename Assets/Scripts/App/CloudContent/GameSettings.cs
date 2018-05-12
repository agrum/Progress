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

				public int NumAbilities { get; private set; }
				public int NumClasses { get; private set; }
				public int NumKits { get; private set; }
				public int LengthConstellation { get; private set; }

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Server.Request(
					HTTPMethods.Get,
					"gameSettings/Classic",
					(JSONNode json_) =>
					{
						Json = json_;

						NumAbilities = Json["numberOfAbilities"];
						NumClasses = Json["numberOfClasses"];
						NumKits = Json["numberOfKits"];
						LengthConstellation = Json["presetLength"];

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

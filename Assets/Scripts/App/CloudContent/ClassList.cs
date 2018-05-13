using System;
using System.Collections.Generic;
using System.Collections;
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
			public class ClassList : Base
			{
				public JSONNode Json { get; private set; } = null;

				public Class this[string uuid_]
				{
					get
					{
						return Table[uuid_] as Class;
					}
				}

				private Hashtable Table = new Hashtable();

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Server.Request(
					HTTPMethods.Get,
					"class",
					(JSONNode json_) =>
					{
						Json = json_;

						foreach (var almostJson in Json)
							Table.Add(almostJson.Value["_id"], new Class(almostJson.Value));

						onBuilt_();
					}).Send();
				}

				public ClassList(GameSettings gameSettings_)
				{
					dependencyList.Add(gameSettings_);
				}
			}
		}
	}
}

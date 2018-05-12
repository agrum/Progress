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
			public class KitList : Base
			{
				public JSONNode Json { get; private set; } = null;

				public Ability this[string uuid_]
				{
					get
					{
						return Table[uuid_] as Ability;
					}
				}

				private Hashtable Table = new Hashtable();

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Server.Request(
					HTTPMethods.Get,
					"kit",
					(JSONNode json_) =>
					{
						Json = json_;

						foreach (var almostSkill in Json)
							Table.Add(almostSkill.Value["_id"], new Class(almostSkill.Value));

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

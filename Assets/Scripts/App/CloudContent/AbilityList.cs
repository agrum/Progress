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
			public class AbilityList : Base
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
					"ability",
					(JSONNode json_) =>
					{
						Json = json_;

						foreach (var almostSkill in Json)
							Table.Add(almostSkill.Value["_id"], new Ability(almostSkill.Value));

						onBuilt_();
					}).Send();
				}

				public AbilityList(GameSettings gameSettings_)
				{
					dependencyList.Add(gameSettings_);
				}
			}
		}
	}
}

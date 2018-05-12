using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using BestHTTP;
using UnityEngine;

namespace West
{
	namespace Model
	{
		namespace CloudContent
		{
			public class Account : Base
			{
				public JSONNode Json { get; private set; } = null;
				public List<ConstellationPreset> PresetList { get; private set; } = new List<ConstellationPreset>();

				protected override void Build(OnBuilt onBuilt_)
				{
					App.Server.Request(
					HTTPMethods.Get,
					"account/" + App.Content.Session.Account,
					(JSONNode json_) =>
					{
						Json = json_;
						PresetList.Clear();
						foreach (var almostPreset in Json["presets"])
							PresetList.Add(new ConstellationPreset(almostPreset.Value));

						Debug.Log(Json);
						onBuilt_();
					}).Send();
				}

				public Account(Constellation constellation_)
				{
					dependencyList.Add(constellation_);
				}
			}
		}
	}
}

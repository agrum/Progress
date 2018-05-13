using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BestHTTP;
using SimpleJSON;
using UnityEngine.UI;

namespace West
{
	namespace Scene
	{
		class PresetEditor : MonoBehaviour
		{
			public View.NodeTextualDetails nodeTextualDetails = null;
			public View.Constellation constellation = null;
			public View.PresetColumn presetColumn = null;
			
			private Model.ConstellationPreset model = null;

			void Start()
			{
				App.Content.Account.Load(() =>
				{
					Setup();
				});
			}

			private void Setup()
			{
				//return if object died while waiting for answer
				if (this == null)
					return;

				//TODO take from save instead of made up
				JSONObject presetJson = new JSONObject();
				presetJson["numAbilities"] = 4;
				presetJson["numKits"] = 1;
				presetJson["numClasses"] = 1;
				presetJson["lengthConstellation"] = 8;
				model = new Model.ConstellationPreset(presetJson);

				nodeTextualDetails.Setup(null);
				constellation.Setup(App.Content.ConstellationList["5aa7523e8d116630ac8cec7c"], model);
				presetColumn.Setup(model, View.PresetColumn.Mode.Edit, nodeTextualDetails);
			}
		}
	}
}

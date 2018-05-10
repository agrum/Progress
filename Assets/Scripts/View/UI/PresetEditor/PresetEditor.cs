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
	namespace View
	{
		class PresetEditor : MonoBehaviour
		{
			public NodeTextualDetails nodeTextualDetails = null;
			public Constellation constellation = null;
			public PresetPreview presetPreview = null;
			
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
				constellation.Setup(model);
				presetPreview.Setup(model);
			}
		}
	}
}

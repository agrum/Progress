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
			public Constellation constellation = null;
			public GameObject prefab = null;
			public Material abilityMaterial = null;
			public Material classMaterial = null;
			public Material kitMaterial = null;

			private bool canStart = false;
			private Model.ConstellationPreset model = null;

			void Start()
			{
				App.Content.Constellation.Load(() => 
				{
					canStart = true;
				});
			}

			void LateUpdate()
			{
				if (model == null && canStart)
					Setup();
			}

			private void Setup()
			{
				//return if object died while waiting for answer
				if (!gameObject)
					return;

				//TODO take from save instead of made up
				JSONObject presetJson = new JSONObject();
				presetJson["numAbilities"] = 4;
				presetJson["numKits"] = 1;
				presetJson["numClasses"] = 1;
				presetJson["lengthConstellation"] = 8;
				model = new Model.ConstellationPreset(presetJson);

				constellation.Setup(model, prefab, abilityMaterial, classMaterial, kitMaterial);
			}
		}
	}
}

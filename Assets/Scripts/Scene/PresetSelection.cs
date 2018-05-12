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
		class PresetSelection : MonoBehaviour
		{
			public View.NodeTextualDetails nodeTextualDetails = null;
			public RectTransform contentElement = null;
			public HorizontalLayoutGroup horizontalLayout = null;
			public GameObject presetColumnPrefab = null;

			private List<View.PresetColumn> presetColumnList = new List<View.PresetColumn>();

			void Start()
			{
				Debug.Assert(nodeTextualDetails != null);
				Debug.Assert(horizontalLayout != null);
				Debug.Assert(presetColumnPrefab != null);

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
				Model.ConstellationPreset model = new Model.ConstellationPreset(presetJson);

				nodeTextualDetails.Setup(null);
				for (int i = 0; i < 1; ++i)
				{
					GameObject gob = Instantiate(presetColumnPrefab);
					View.PresetColumn presetColumn = gob.GetComponent<View.PresetColumn>();
					presetColumn.Setup(model, View.PresetColumn.Mode.Display, nodeTextualDetails);
					presetColumn.transform.SetParent(horizontalLayout.transform, false);
					presetColumnList.Add(presetColumn);
					contentElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150.0f * presetColumnList.Count);
				}
			}
		}
	}
}

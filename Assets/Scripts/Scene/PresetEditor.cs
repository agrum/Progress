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

            static public Model.ConstellationPreset Model = null;

			void Start()
			{
                if (Model == null)
                {
                    Debug.Log("Model is null in PresetEditor");
                    return;
                }

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

				nodeTextualDetails.Setup(null);
				constellation.Setup(Model.Constellation, Model);
				presetColumn.Setup(Model, View.PresetColumn.Mode.Edit, nodeTextualDetails);
			}
		}
	}
}

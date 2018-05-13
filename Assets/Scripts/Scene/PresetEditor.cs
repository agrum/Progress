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
				
				model = App.Content.Account.EditedPreset;

				nodeTextualDetails.Setup(null);
				constellation.Setup(model.Constellation, model);
				presetColumn.Setup(model, View.PresetColumn.Mode.Edit, nodeTextualDetails);
			}
		}
	}
}

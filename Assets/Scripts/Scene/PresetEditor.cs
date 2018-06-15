using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace West
{
	namespace Scene
	{
		class PresetEditor : MonoBehaviour
        {
            public View.NodeTextualDetails nodeTextualDetails = null;
            public View.NodeMap constellation = null;
			public View.PresetColumn presetColumn = null;
            public Button backButton = null;

            static public Model.ConstellationPreset Model = null;

			private Model.HoveredSkill hovered = new Model.HoveredSkill();

			void Start()
			{
				Debug.Assert(backButton != null);
				Debug.Assert(Model != null);

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

                backButton.onClick.AddListener(BackClicked);
                nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
				constellation.SetContext(new ViewModel.Constellation(Model.Constellation, Model));
				presetColumn.SetContext(new ViewModel.PresetColumn(Model, hovered, ViewModel.PresetColumn.Mode.Edit));
				
				App.Content.Account.PresetSaved += OnPresetSaved;
			}

			private void OnPresetSaved(Model.ConstellationPreset preset)
			{
				GameObject.Instantiate(Resources.Load("Prefabs/LoadingCanvas", typeof(GameObject)));
				SceneManager.LoadScene("PresetSelection");
				Model = null;
			}


			private void BackClicked()
            {
                SceneManager.LoadScene("PresetSelection");
            }
        }
	}
}

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
            public View.Constellation constellation = null;
			public View.PresetColumn presetColumn = null;
            public Button backButton = null;

            static public Model.ConstellationPreset Model = null;

			private ViewModel.Constellation viewModelConstellation = null;
			private ViewModel.PresetColumn viewModelPresetColumn = null;

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
                nodeTextualDetails.Setup(null, null);
				viewModelConstellation = new ViewModel.Constellation(constellation, Model.Constellation, Model);
				viewModelPresetColumn = new ViewModel.PresetColumn(presetColumn, nodeTextualDetails, Model, ViewModel.PresetColumn.Mode.Edit);
            }

            private void BackClicked()
            {
                SceneManager.LoadScene("PresetSelection");
            }
        }
	}
}

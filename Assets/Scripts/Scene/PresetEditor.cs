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

			void Start()
            {
                Debug.Assert(backButton != null);

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

                backButton.onClick.AddListener(BackClicked);
                nodeTextualDetails.Setup(null);
				constellation.Setup(Model.Constellation, Model);
				presetColumn.Setup(Model, View.PresetColumn.Mode.Edit, nodeTextualDetails);
            }

            private void BackClicked()
            {
                SceneManager.LoadScene("PresetSelection");
            }
        }
	}
}

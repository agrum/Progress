using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace West
{
	namespace Scene
	{
		class PresetSelection : MonoBehaviour
		{
			public Canvas canvas = null;
			public View.NodeTextualDetails nodeTextualDetails = null;
			public RectTransform contentElement = null;
			public HorizontalLayoutGroup horizontalLayout = null;
			public GameObject presetColumnPrefab = null;
            public Button backButton = null;

            private List<View.PresetColumn> presetColumnList = new List<View.PresetColumn>();

			void Start()
			{
				Debug.Assert(canvas != null);
				Debug.Assert(nodeTextualDetails != null);
				Debug.Assert(horizontalLayout != null);
                Debug.Assert(presetColumnPrefab != null);
                Debug.Assert(backButton != null);
				
				canvas.gameObject.SetActive(false);

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
                backButton.onClick.AddListener(BackClicked);

                //setup existing preset columns
                foreach (var preset in App.Content.Account.PresetList)
				{
					GameObject gob = Instantiate(presetColumnPrefab);
					View.PresetColumn presetColumn = gob.GetComponent<View.PresetColumn>();
                    presetColumn.ColumnDestroyedEvent += OnPresetRemoved;

                    presetColumn.Setup(preset, View.PresetColumn.Mode.Display, nodeTextualDetails);
					presetColumn.transform.SetParent(horizontalLayout.transform, false);
					presetColumnList.Add(presetColumn);
				}

                //add empty column to add presets.
                {
                    GameObject gob = Instantiate(presetColumnPrefab);
                    View.PresetColumn presetColumn = gob.GetComponent<View.PresetColumn>();
                    presetColumn.Setup(null, View.PresetColumn.Mode.Display, nodeTextualDetails);
                    presetColumn.transform.SetParent(horizontalLayout.transform, false);
                    presetColumnList.Add(presetColumn);
                }

                ArrangeUI();

				canvas.gameObject.SetActive(true);
			}

            private void OnPresetRemoved(View.PresetColumn column_)
            {
                presetColumnList.Remove(column_);
                Destroy(column_.gameObject);
                column_.ColumnDestroyedEvent -= OnPresetRemoved;
                ArrangeUI();
            }

            private void ArrangeUI()
            {
                contentElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150.0f * presetColumnList.Count);
            }

            private void BackClicked()
            {
                SceneManager.LoadScene("Landing");
            }
        }
	}
}

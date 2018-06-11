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

			private List<ViewModel.PresetColumn> presetColumnList = new List<ViewModel.PresetColumn>();

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

				nodeTextualDetails.Setup(null, null);
				backButton.onClick.AddListener(BackClicked);

				//setup existing preset columns
				foreach (var preset in App.Content.Account.PresetList)
				{
					GameObject gob = Instantiate(presetColumnPrefab);
					View.PresetColumn viewPresetColumn = gob.GetComponent<View.PresetColumn>();
					viewPresetColumn.transform.SetParent(horizontalLayout.transform, false);

					ViewModel.PresetColumn presetColumn = new ViewModel.PresetColumn(
						viewPresetColumn,
						nodeTextualDetails,
						preset,
						ViewModel.PresetColumn.Mode.Display);
					presetColumn.ColumnDestroyedEvent += OnPresetRemoved;
					presetColumnList.Add(presetColumn);
				}

				//add empty column to add presets.
				{
					GameObject gob = Instantiate(presetColumnPrefab);
					View.PresetColumn viewPresetColumn = gob.GetComponent<View.PresetColumn>();
					viewPresetColumn.transform.SetParent(horizontalLayout.transform, false);

					ViewModel.PresetColumn presetColumn = new ViewModel.PresetColumn(
						viewPresetColumn,
						nodeTextualDetails,
						null,
						ViewModel.PresetColumn.Mode.Display);
					presetColumnList.Add(presetColumn);
				}

				ArrangeUI();

				canvas.gameObject.SetActive(true);
			}

			void OnDestroy()
			{
				presetColumnList.Clear();
			}

            private void OnPresetRemoved(ViewModel.PresetColumn column_)
            {
                presetColumnList.Remove(column_);
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

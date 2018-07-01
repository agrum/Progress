using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
	class PresetSelection : MonoBehaviour
	{
		public Canvas canvas = null;
		public View.NodeTextualDetails nodeTextualDetails = null;
		public RectTransform contentElement = null;
		public HorizontalLayoutGroup horizontalLayout = null;
		public GameObject presetColumnPrefab = null;
		public Button backButton = null;
			
		private Model.HoveredSkill hovered = new Model.HoveredSkill();

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

			nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
			backButton.onClick.AddListener(BackClicked);

			//setup existing preset columns
			foreach (var preset in App.Content.Account.ActiveChampion.PresetList)
			{
				GameObject gob = Instantiate(presetColumnPrefab);
				gob.transform.SetParent(horizontalLayout.transform, false);
				View.PresetColumn viewPresetColumn = gob.GetComponent<View.PresetColumn>();

				viewPresetColumn.SetContext(new ViewModel.PresetColumn(
					preset,
					hovered,
					ViewModel.PresetColumn.Mode.Display));
			}

			//add empty column to add presets.
			{
				GameObject gob = Instantiate(presetColumnPrefab);
				gob.transform.SetParent(horizontalLayout.transform, false);
				View.PresetColumn viewPresetColumn = gob.GetComponent<View.PresetColumn>();

				viewPresetColumn.SetContext(new ViewModel.PresetColumn(
					null,
					hovered,
					ViewModel.PresetColumn.Mode.Addition));
			}

			ArrangeUI();

			App.Content.Account.ActiveChampion.PresetAdded += OnPresetAdded;
			App.Content.Account.ActiveChampion.PresetRemoved += OnPresetRemoved;

			canvas.gameObject.SetActive(true);
		}

		void OnDestroy()
		{
			App.Content.Account.ActiveChampion.PresetAdded -= OnPresetAdded;
			App.Content.Account.ActiveChampion.PresetRemoved -= OnPresetRemoved;
		}

		private void OnPresetAdded(Model.ConstellationPreset preset)
		{
			PresetEditor.Model = preset;
            App.Scene.Load("PresetEditor");
		}

		private void OnPresetRemoved(Model.ConstellationPreset preset)
		{
			ArrangeUI();
		}

		private void ArrangeUI()
        {
            contentElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150.0f * (App.Content.Account.ActiveChampion.PresetList.Count + 1));
        }

        private void BackClicked()
        {
            App.Scene.Load("ChampionSelection");
        }
    }
}

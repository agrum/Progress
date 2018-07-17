using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	class PresetColumn : WestBehaviour
	{
		public NodeMap presetPreview = null;
        public Button addButton = null;
		public Button editButton = null;
		public Button deleteButton = null;
		public Button proceedButton = null;
		public Button saveButton = null;
        public WestText handicapText = null;
        public Text nameText = null;
        public InputField nameInput = null;

		private ViewModel.PresetColumn viewModel;

		public void SetContext(ViewModel.PresetColumn viewModel_)
		{
			Debug.Assert(viewModel_ != null);

			viewModel = viewModel_;
            viewModel.PresetDestroyed += OnPresetDestroyed;
            viewModel.PresetUpdated += OnPresetUpdated;

            presetPreview.SetContext(viewModel.CreatePreviewContext());

            Delay(DelayedStart);
		}

        private void DelayedStart()
		{
			Debug.Assert(presetPreview != null);
            Debug.Assert(addButton != null);
            Debug.Assert(editButton != null);
			Debug.Assert(deleteButton != null);
			Debug.Assert(proceedButton != null);
			Debug.Assert(saveButton != null);
            Debug.Assert(handicapText != null);
            Debug.Assert(nameText != null);
            Debug.Assert(nameInput != null);

			DisableAll();
                
            addButton.onClick.AddListener(viewModel.AddClicked);
            editButton.onClick.AddListener(viewModel.EditClicked);
			deleteButton.onClick.AddListener(viewModel.DeleteClicked);
			proceedButton.onClick.AddListener(viewModel.ProceedClicked);
			saveButton.onClick.AddListener(viewModel.SaveClicked);
            nameInput.onEndEdit.AddListener(viewModel.NameChanged);

			if (viewModel.mode == ViewModel.PresetColumn.Mode.Addition)
				SetModeAddition();
			else if (viewModel.mode == ViewModel.PresetColumn.Mode.Display)
				SetModeDisplay(viewModel.preset.Name);
			else
				SetModeEdit(viewModel.preset.Name);
		}
			
		private void OnPresetDestroyed()
		{
			Destroy(gameObject);
		}

		private void OnDestroy()
        {
            if (viewModel == null)
                return;

            viewModel.PresetDestroyed -= OnPresetDestroyed;
            viewModel.PresetUpdated -= OnPresetUpdated;
            viewModel = null;
		}

		private void SetModeAddition()
		{
			DisableAll();
			addButton.gameObject.SetActive(true);
		}

		private void SetModeDisplay(string name)
		{
			DisableAll();
			nameText.text = name;
            handicapText.Format(viewModel.HandicapLevel().ToString(), viewModel.HandicapPercentage().ToString());
			presetPreview.gameObject.SetActive(true);
			editButton.gameObject.SetActive(true);
			deleteButton.gameObject.SetActive(true);
            handicapText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
			proceedButton.gameObject.SetActive(true);
		}

		private void SetModeEdit(string name)
		{
			DisableAll();
			nameInput.text = name;
            handicapText.Format(viewModel.HandicapLevel().ToString(), viewModel.HandicapPercentage().ToString());
            presetPreview.gameObject.SetActive(true);
            handicapText.gameObject.SetActive(true);
            nameInput.gameObject.SetActive(true);
			saveButton.gameObject.SetActive(true);
		}

		private void DisableAll()
		{
			presetPreview.gameObject.SetActive(false);
			addButton.gameObject.SetActive(false);
			editButton.gameObject.SetActive(false);
			deleteButton.gameObject.SetActive(false);
			proceedButton.gameObject.SetActive(false);
			saveButton.gameObject.SetActive(false);
            handicapText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
            nameInput.gameObject.SetActive(false);
        }

        private void OnPresetUpdated()
        {
            handicapText.Format(viewModel.HandicapLevel().ToString(), viewModel.HandicapPercentage().ToString());
        }

    }
}

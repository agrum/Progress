using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;

namespace West
{
	namespace View
	{
		class PresetColumn : WestBehaviour
		{
			public enum Mode
			{
				Display,
				Edit
			}

			public PresetPreview presetPreview = null;
			public Button addButton = null;
			public Button editButton = null;
			public Button deleteButton = null;
			public Button proceedButton = null;
			public Button saveButton = null;
			public Text nameText = null;
			public InputField nameInput = null;

			public Model.ConstellationPreset Model { get; private set; } = null;

            public delegate void OnColumnDestroyedDelegate(PresetColumn column_);
            public event OnColumnDestroyedDelegate ColumnDestroyedEvent;

            protected override void Start()
			{
				Debug.Assert(presetPreview != null);
				Debug.Assert(addButton != null);
				Debug.Assert(editButton != null);
				Debug.Assert(deleteButton != null);
				Debug.Assert(proceedButton != null);
				Debug.Assert(saveButton != null);
				Debug.Assert(nameText != null);
				Debug.Assert(nameInput != null);

				DisableAll();

				addButton.onClick.AddListener(AddClicked);
				editButton.onClick.AddListener(EditClicked);
				deleteButton.onClick.AddListener(DeleteClicked);
				proceedButton.onClick.AddListener(ProceedClicked);
				saveButton.onClick.AddListener(SaveClicked);

				base.Start();
			}

			public void Setup(Model.ConstellationPreset model_, Mode mode_, NodeTextualDetails nodeTextualDetails_)
			{
				SetupOnStarted(() =>
				{
                    Model = model_;

                    DisableAll();
					if (Model == null)
					{
						if (mode_ == Mode.Display)
							addButton.gameObject.SetActive(true);
						else
							Debug.Log("PresetColumn.Setup() not supported");
					}
					else
					{
						presetPreview.Setup(Model, nodeTextualDetails_);
						if (mode_ == Mode.Display)
						{
							nameText.text = Model.Name;

							presetPreview.gameObject.SetActive(true);
							editButton.gameObject.SetActive(true);
							deleteButton.gameObject.SetActive(true);
							nameText.gameObject.SetActive(true);
							proceedButton.gameObject.SetActive(true);
						}
						else
						{
							nameInput.text = Model.Name;

							presetPreview.gameObject.SetActive(true);
							nameInput.gameObject.SetActive(true);
							saveButton.gameObject.SetActive(true);
						}
					}
				});
			}

			private void DisableAll()
			{
				presetPreview.gameObject.SetActive(false);
				addButton.gameObject.SetActive(false);
				editButton.gameObject.SetActive(false);
				deleteButton.gameObject.SetActive(false);
				proceedButton.gameObject.SetActive(false);
				saveButton.gameObject.SetActive(false);
				nameText.gameObject.SetActive(false);
				nameInput.gameObject.SetActive(false);
			}

			private void AddClicked()
			{
				DisableAll();
				App.Content.Account.AddPreset((Model.ConstellationPreset preset_) =>
				{
					App.Content.Account.EditPreset(preset_);
				});
			}

			private void EditClicked()
			{
				App.Content.Account.EditPreset(Model);
			}

			private void DeleteClicked()
            {
                DisableAll();
                App.Content.Account.RemovePreset(Model, () =>
                {
                    this.ColumnDestroyedEvent(this);
                });
            }

			private void ProceedClicked()
			{
				//nothing yet
			}

			private void SaveClicked()
			{
				//ask account to overwrite save model with local copy
			}
		}
	}
}

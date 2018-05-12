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

			public Model.ConstellationPreset model { get; private set; } = null;

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
					DisableAll();
					if (model_ == null)
					{
						if (mode_ == Mode.Display)
							addButton.gameObject.SetActive(true);
						else
							Debug.Log("PresetColumn.Setup() not supported");
					}
					else
					{
						presetPreview.Setup(model_, nodeTextualDetails_);
						if (mode_ == Mode.Display)
						{
							nameText.text = model_.Name;

							presetPreview.gameObject.SetActive(true);
							editButton.gameObject.SetActive(true);
							deleteButton.gameObject.SetActive(true);
							nameText.gameObject.SetActive(true);
							proceedButton.gameObject.SetActive(true);
						}
						else
						{
							nameInput.text = model_.Name;

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
				//add preset to account (account always returns copies, not originals)

				//setup with new created model

				//edit
			}

			private void EditClicked()
			{
				//open PresetEditor scene with active model
			}

			private void DeleteClicked()
			{
				//delete preset in account

				//delete self game object
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

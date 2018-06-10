using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;
using UnityEngine.SceneManagement;

namespace West
{
	namespace View
	{
		class PresetColumn : MonoBehaviour
		{
			public delegate void OnStartedDelegate();
			public event OnStartedDelegate StartedEvent = delegate { };
			public delegate void OnButtonClickedDelegate();
			public event OnButtonClickedDelegate AddClickedEvent = delegate { };
			public event OnButtonClickedDelegate EditClickedEvent = delegate { };
			public event OnButtonClickedDelegate DeleteClickedEvent = delegate { };
			public event OnButtonClickedDelegate ProceedClickedEvent = delegate { };
			public event OnButtonClickedDelegate SaveClickedEvent = delegate { };
			public delegate void OnInputFinishedDelegate(string text);
			public event OnInputFinishedDelegate NameChanegdEvent = delegate { };

			public PresetPreview presetPreview = null;
            public Button addButton = null;
			public Button editButton = null;
			public Button deleteButton = null;
			public Button proceedButton = null;
			public Button saveButton = null;
			public Text nameText = null;
			public InputField nameInput = null;

			public Model.ConstellationPreset Model { get; private set; } = null;

            protected void Start()
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
                nameInput.onEndEdit.AddListener(NameChanged);

				StartedEvent();
			}

			public void SetModeAddition()
			{
				DisableAll();
				addButton.gameObject.SetActive(true);
			}

			public void SetModeDisplay(string name)
			{
				DisableAll();
				nameText.text = name;
				presetPreview.gameObject.SetActive(true);
				editButton.gameObject.SetActive(true);
				deleteButton.gameObject.SetActive(true);
				nameText.gameObject.SetActive(true);
				proceedButton.gameObject.SetActive(true);
			}

			public void SetModeEdit(string name)
			{
				DisableAll();
				nameInput.text = name;
				presetPreview.gameObject.SetActive(true);
				nameInput.gameObject.SetActive(true);
				saveButton.gameObject.SetActive(true);
			}

			public void DisableAll()
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
				AddClickedEvent();
            }

            private void EditClicked()
			{
				EditClickedEvent();
            }

			private void DeleteClicked()
			{
				DeleteClickedEvent();
			}

			private void ProceedClicked()
			{
				ProceedClickedEvent();
			}

			private void SaveClicked()
			{
				SaveClickedEvent();
			}

            private void NameChanged(string newName)
			{
				NameChanegdEvent(newName);
			}
		}
	}
}

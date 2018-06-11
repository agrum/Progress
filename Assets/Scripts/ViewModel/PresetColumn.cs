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
	namespace ViewModel
	{
		class PresetColumn
		{
			public enum Mode
			{
				Display,
				Edit
			}

			public delegate void OnColumnDestroyedDelegate(PresetColumn column_);
			public event OnColumnDestroyedDelegate ColumnDestroyedEvent;

			private View.PresetColumn view = null;
			private View.NodeTextualDetails details = null;
			public Model.ConstellationPreset Model { get; private set; } = null;
			private Mode mode;

			public PresetColumn(
				View.PresetColumn view_,
				View.NodeTextualDetails nodeTextualDetails_,
				Model.ConstellationPreset model_,
				Mode mode_)
			{
				view = view_;
				details = nodeTextualDetails_;
				Model = model_;
				mode = mode_;

				view.AddClickedEvent += AddClicked;
				view.EditClickedEvent += EditClicked;
				view.DeleteClickedEvent += DeleteClicked;
				view.ProceedClickedEvent += ProceedClicked;
				view.SaveClickedEvent += SaveClicked;
				view.NameChanegdEvent += NameChanged;
				view.StartedEvent += ViewStarted;
			}

			~PresetColumn()
			{
				Debug.Log("~PresetColumn()");
			}

			private void ViewStarted()
			{
				if (Model == null)
				{
					if (mode == Mode.Display)
						view.SetModeAddition();
					else
						Debug.Log("PresetColumn.Setup() not supported");
				}
				else
				{
					if (mode == Mode.Display)
					{
						view.SetModeDisplay(Model.Name);
						view.gameObject.AddComponent<PresetPreview>().Setup(view.presetPreview, details, Model, false);
					}
					else
					{
						view.SetModeEdit(Model.Name);
						view.gameObject.AddComponent<PresetPreview>().Setup(view.presetPreview, details, Model, true);
					}
				}
			}

			private void AddClicked()
			{
				view.DisableAll();
				App.Content.Account.AddPreset((Model.ConstellationPreset preset_) =>
				{
					Model = preset_;
					EditClicked();
				});
			}

			private void EditClicked()
			{
				view.DisableAll();
				if (!App.Content.Account.PresetList.Contains(Model))
					return;

				Scene.PresetEditor.Model = Model;
				GameObject.Instantiate(Resources.Load("Prefabs/LoadingCanvas", typeof(GameObject)));
				SceneManager.LoadScene("PresetEditor");
			}

			private void DeleteClicked()
			{
				view.DisableAll();
				App.Content.Account.RemovePreset(Model, () =>
				{
					view.AddClickedEvent -= AddClicked;
					view.EditClickedEvent -= EditClicked;
					view.DeleteClickedEvent -= DeleteClicked;
					view.ProceedClickedEvent -= ProceedClicked;
					view.SaveClickedEvent -= SaveClicked;
					view.NameChanegdEvent -= NameChanged;
					view.StartedEvent -= ViewStarted;
					view.transform.SetParent(null, false);
					GameObject.Destroy(view.gameObject);
					view = null;

					ColumnDestroyedEvent(this);
				});
			}

			private void ProceedClicked()
			{
				//nothing yet
			}

			private void SaveClicked()
			{
				view.DisableAll();
				App.Content.Account.SavePreset(Model, () =>
				{
					SceneManager.LoadScene("PresetSelection");
					Scene.PresetEditor.Model = null;
				});
			}

			private void NameChanged(string newName)
			{
				Model.Name = newName;
			}
		}
	}
}

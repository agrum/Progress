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
				Addition,
				Display,
				Edit
			}
			public event OnVoidDelegate PresetDestroyed = delegate { };

			public delegate void OnColumnDestroyedDelegate(PresetColumn column_);
			public event OnColumnDestroyedDelegate ColumnDestroyedEvent;
			
			public Model.ConstellationPreset preset;
			public Model.HoveredSkill hovered;
			public Mode mode;

			public PresetColumn(
				Model.ConstellationPreset model_,
				Model.HoveredSkill hovered_,
				Mode mode_)
			{
				preset = model_;
				hovered = hovered_;
				mode = mode_;
			}

			~PresetColumn()
			{
				Debug.Log("~PresetColumn()");
			}

			public void AddClicked()
			{
				App.Content.Account.AddPreset((Model.ConstellationPreset preset_) =>
				{
					preset = preset_;
					EditClicked();
				});
			}

			public void EditClicked()
			{
				if (!App.Content.Account.PresetList.Contains(preset))
					return;

				Scene.PresetEditor.Model = preset;
				GameObject.Instantiate(Resources.Load("Prefabs/LoadingCanvas", typeof(GameObject)));
				SceneManager.LoadScene("PresetEditor");
			}

			public void DeleteClicked()
			{
				App.Content.Account.RemovePreset(preset, () =>
				{
					PresetDestroyed();
				});
			}

			public void ProceedClicked()
			{
				//nothing yet
			}

			public void SaveClicked()
			{
				App.Content.Account.SavePreset(preset, () =>
				{
					SceneManager.LoadScene("PresetSelection");
					Scene.PresetEditor.Model = null;
				});
			}

			public void NameChanged(string newName)
			{
				preset.Name = newName;
			}
		}
	}
}

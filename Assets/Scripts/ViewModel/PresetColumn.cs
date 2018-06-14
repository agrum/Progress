using UnityEngine;
using UnityEngine.SceneManagement;

namespace West.ViewModel
{
	interface IPresetColumn
	{
		event OnVoidDelegate PresetDestroyed;
	}

	class PresetColumn : IPresetColumn
	{
		public enum Mode
		{
			Addition,
			Display,
			Edit
		}
		public event OnVoidDelegate PresetDestroyed = delegate { };
			
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

			App.Content.Account.PresetRemoved += OnPresetRemoved;
		}

		~PresetColumn()
		{
			App.Content.Account.PresetRemoved -= OnPresetRemoved;
		}

		public INodeMap CreatePreviewContext()
		{
			return new PresetPreview(
					preset,
					hovered,
					mode == Mode.Edit);
		}

		public void AddClicked()
		{
			App.Content.Account.AddPreset();
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
			App.Content.Account.RemovePreset(preset);
		}

		public void ProceedClicked()
		{
			//nothing yet
		}

		public void SaveClicked()
		{
			App.Content.Account.SavePreset(preset);
		}

		public void NameChanged(string newName)
		{
			preset.Name = newName;
		}

		private void OnPresetRemoved(Model.ConstellationPreset preset_)
		{
			if (preset == preset_)
			{
				PresetDestroyed();
			}
		}
	}
}
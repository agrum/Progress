using UnityEngine;

namespace Assets.Scripts.ViewModel
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

            if (App.Content.Account.ActiveChampion == null)
			    App.Content.Account.ActiveChampion.PresetRemoved += OnPresetRemoved;
		}

		~PresetColumn()
		{
            if (App.Content.Account.ActiveChampion == null)
                App.Content.Account.ActiveChampion.PresetRemoved -= OnPresetRemoved;
		}

		public INodeMap CreatePreviewContext()
		{
            if (preset != null)
                return new NodeMapPreset(
                        preset,
                        hovered,
                        mode == Mode.Edit);
            else
                return new NodeMapEmpty();
		}

		public void AddClicked()
		{
            Scene.PresetEditor.Model = null;
            App.Scene.Load("PresetEditor");
            //App.Content.Account.ActiveChampion.AddEmptyPreset();
		}

		public void EditClicked()
		{
			if (App.Content.Account.ActiveChampion == null || !App.Content.Account.ActiveChampion.PresetList.Contains(preset))
				return;

			Scene.PresetEditor.Model = preset;
            App.Scene.Load("PresetEditor");
        }

		public void DeleteClicked()
		{
			App.Content.Account.ActiveChampion?.RemovePreset(preset);
		}

		public void ProceedClicked()
		{
			//nothing yet
		}

		public void SaveClicked()
		{
			App.Content.Account.ActiveChampion?.SavePreset(preset);
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
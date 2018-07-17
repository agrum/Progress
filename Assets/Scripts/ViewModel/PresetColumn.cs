using UnityEngine;

namespace Assets.Scripts.ViewModel
{
	interface IPresetColumn
    {
        event OnVoidDelegate PresetDestroyed;
        event OnVoidDelegate PresetUpdated;

        int HandicapLevel();
        float HandicapPercentage();
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
        public event OnVoidDelegate PresetUpdated = delegate { };

        public Model.ConstellationPreset preset;
        public Model.HoveredSkill hovered;
        public Model.Champion champion;
        public Mode mode;

		public PresetColumn(
			Model.ConstellationPreset model_,
			Model.HoveredSkill hovered_,
			Mode mode_)
		{
			preset = model_;
			hovered = hovered_;
            champion = App.Content.Account.ActiveChampion;
            mode = mode_;

            if (preset != null)
                preset.PresetUpdated += OnPresetUpdated;

            if (champion != null)
                champion.PresetRemoved += OnPresetRemoved;
		}

		~PresetColumn()
		{
            if (preset != null)
                preset.PresetUpdated -= OnPresetUpdated;

            if (champion != null)
                champion.PresetRemoved -= OnPresetRemoved;
		}

        public int HandicapLevel()
        {
            if (champion == null)
                return 0;

            int cumulativeHandicap = 0;
            foreach (var skill in preset.SelectedAbilityList)
                cumulativeHandicap += champion.Upgrades[skill].Handicap();
            foreach (var skill in preset.SelectedClassList)
                cumulativeHandicap += champion.Upgrades[skill].Handicap();
            foreach (var skill in preset.SelectedKitList)
                cumulativeHandicap += champion.Upgrades[skill].Handicap();

            return cumulativeHandicap;
        }

        public float HandicapPercentage()
        {
            double handicap = HandicapLevel();
            double transient = handicap / 40.0f;
            if (transient >= 0)
                transient = 1.0 / (1.0 + transient);
            else
                transient = 1.0 - transient;
            return (float) System.Math.Sqrt(transient);
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
			if (champion == null || !champion.PresetList.Contains(preset))
				return;

			Scene.PresetEditor.Model = preset;
            App.Scene.Load("PresetEditor");
        }

		public void DeleteClicked()
		{
            champion?.RemovePreset(preset);
		}

		public void ProceedClicked()
		{
			//nothing yet
		}

		public void SaveClicked()
		{
            if (preset.Name == "" || preset.Name == null)
            {
                App.Resource.Prefab.Popup().Setup("Error", "Preset requires a name.");
                return;
            }

            champion?.SavePreset(preset);
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

        private void OnPresetUpdated()
        {
            PresetUpdated();
        }
    }
}
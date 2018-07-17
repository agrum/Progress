using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts.Scene
{
	class PresetEditor : MonoBehaviour
    {
        public View.NodeTextualDetails nodeTextualDetails = null;
        public View.NodeMap constellation = null;
		public View.PresetColumn presetColumn = null;
        public Button backButton = null;

        static public Model.ConstellationPreset Model = null;

		private Model.HoveredSkill hovered = new Model.HoveredSkill();
        private Model.Champion champion = null;

		void Start()
		{
			Debug.Assert(backButton != null);

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

            if (Model == null)
            {
                Model = new Model.ConstellationPreset(
                        null,
                        new Model.PresetLimits(App.Content.GameSettings.NumAbilities, App.Content.GameSettings.NumClasses, App.Content.GameSettings.NumKits));
            }

            champion = App.Content.Account.ActiveChampion;
            List<Model.Skill> filteredSkillList = new List<Model.Skill>();
            foreach (var node in Model.Constellation.AbilityNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var node in Model.Constellation.KitNodeList)
                filteredSkillList.Add(node.Skill);
            if (champion != null)
                foreach (var skill in champion.ClassPreset.SelectedClassList)
                    filteredSkillList.Add(skill);
            else
                foreach (var node in Model.Constellation.ClassNodeList)
                    filteredSkillList.Add(node.Skill);

            backButton.onClick.AddListener(BackClicked);
            nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
			constellation.SetContext(new ViewModel.NodeMapConstellation(Model.Constellation, Model, filteredSkillList, hovered));
			presetColumn.SetContext(new ViewModel.PresetColumn(Model, hovered, ViewModel.PresetColumn.Mode.Edit));

            if (champion != null)
                champion.PresetSaved += OnPresetSaved;
		}

        void OnDestroy()
        {
            if (champion != null)
                champion.PresetSaved -= OnPresetSaved;
        }

		private void OnPresetSaved(Model.ConstellationPreset preset)
        {
            App.Scene.Load("PresetSelection");
			Model = null;
		}


		private void BackClicked()
        {
            App.Scene.Load("PresetSelection");
        }
    }
}

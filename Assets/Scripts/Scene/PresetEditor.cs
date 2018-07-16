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

		void Start()
		{
			Debug.Assert(backButton != null);

            if (Model == null)
            {
                Model = new Model.ConstellationPreset(
                        null,
                        new Model.PresetLimits(App.Content.GameSettings.NumAbilities, App.Content.GameSettings.NumClasses, App.Content.GameSettings.NumKits));
            }

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

            var champion = App.Content.Account.ActiveChampion;
            List<Model.Skill> filteredSkillList = new List<Model.Skill>();
            foreach (var node in Model.Constellation.AbilityNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var node in Model.Constellation.KitNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var skill in champion.ClassPreset.SelectedClassList)
                filteredSkillList.Add(skill);

            backButton.onClick.AddListener(BackClicked);
            nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
			constellation.SetContext(new ViewModel.NodeMapConstellation(Model.Constellation, Model, filteredSkillList, hovered));
			presetColumn.SetContext(new ViewModel.PresetColumn(Model, hovered, ViewModel.PresetColumn.Mode.Edit));
				
			App.Content.Account.ActiveChampion.PresetSaved += OnPresetSaved;
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

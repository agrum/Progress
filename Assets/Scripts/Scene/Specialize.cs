using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts.Scene
{
    class Specialize : MonoBehaviour
    {
        public View.NodeTextualDetails nodeTextualDetails = null;
        public View.SkillSpecializer specializer = null;
        public Button backButton = null;
        public View.ChampionHeadline championHeadline = null;

        public static Model.Skill SelectedSkill = null;
        
        private Model.HoveredSkill hovered = new Model.HoveredSkill();

        void Start()
        {
            Debug.Assert(nodeTextualDetails != null);
            Debug.Assert(specializer != null);
            Debug.Assert(backButton != null);
            Debug.Assert(championHeadline != null);
            Debug.Assert(SelectedSkill != null);

            var loadingScreen = App.Resource.Prefab.LoadingCanvas();
                
            App.Content.Account.ActiveChampion?.Load(() =>
            {
                Destroy(loadingScreen);

                Setup();
            });
        }

        private void Setup()
        {
            if (this == null)
                return;

            //model
            var constellationModel = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
            List<Model.Skill> filteredSkillList = new List<Model.Skill>();
            foreach (var node in constellationModel.AbilityNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var node in constellationModel.KitNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var skill in App.Content.Account.ActiveChampion.ClassPreset.SelectedClassList)
                filteredSkillList.Add(skill);
            hovered.Skill = SelectedSkill;

            //view
            backButton.onClick.AddListener(() => { App.Scene.Load("SpecializeOverview"); });
            nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
            specializer.SetContext(new ViewModel.SkillSpecializer(App.Content.Account.ActiveChampion.Upgrades[SelectedSkill], true));
            championHeadline.Setup();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts.Scene
{
    class ChampionCreator : MonoBehaviour
    {
        public View.NodeTextualDetails nodeTextualDetails = null;
        public View.NodeMap constellation = null;
        public View.ChampionColumnCreate championColumn = null;
        public Button backButton = null;

        private Model.ConstellationPreset preset = null;
        private Model.HoveredSkill hovered = new Model.HoveredSkill();

        IEnumerator Start()
        {
            Debug.Assert(backButton != null);

            yield return StartCoroutine(App.Content.ConstellationList.Load());
            
            //return if object died while waiting for answer
            if (this == null)
                yield break;

            var constellationModel = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
            List<Data.Skill.Skill> filteredSkillList = new List<Data.Skill.Skill>();
            foreach (var node in constellationModel.ClassNodeList)
                filteredSkillList.Add(node.Skill);

            preset = new Model.ConstellationPreset(new SimpleJSON.JSONObject(), new Model.PresetLimits(0, 3, 0));
            backButton.onClick.AddListener(BackClicked);
            nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
            constellation.SetContext(new ViewModel.NodeMapConstellation(
                constellationModel, 
                preset,
                filteredSkillList,
                hovered));
            championColumn.SetContext(new ViewModel.ChampionColumnCreate(preset, hovered));

            App.Content.Account.ChampionAdded += OnChampionAdded;
        }

        private void OnChampionAdded(Model.Champion champion)
        {
            App.Content.Account.ActivateChampion(champion);
            App.Scene.Load("Landing");
        }


        private void BackClicked()
        {
            App.Scene.Load("ChampionSelection");
        }
    }
}

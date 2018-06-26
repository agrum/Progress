using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace West
{
    namespace Scene
    {
        class ChampionCreator : MonoBehaviour
        {
            public View.NodeTextualDetails nodeTextualDetails = null;
            public View.NodeMap constellation = null;
            public View.ChampionColumnCreate championColumn = null;
            public Button backButton = null;

            private Model.ConstellationPreset preset = null;
            private Model.HoveredSkill hovered = new Model.HoveredSkill();

            void Start()
            {
                Debug.Assert(backButton != null);

                App.Content.ConstellationList.Load(() =>
                {
                    Setup();
                });
            }

            private void Setup()
            {
                //return if object died while waiting for answer
                if (this == null)
                    return;

                var constellationModel = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
                List<Model.Skill> filteredSkillList = new List<Model.Skill>();
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
                App.Resource.Prefab.LoadingCanvas();
                SceneManager.LoadScene("PresetSelection");
            }


            private void BackClicked()
            {
                App.Resource.Prefab.LoadingCanvas();
                SceneManager.LoadScene("ChampionSelection");
            }
        }
    }
}

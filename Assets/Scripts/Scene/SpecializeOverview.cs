﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts.Scene
{
    class SpecializeOverview : MonoBehaviour
    {
        public View.NodeTextualDetails nodeTextualDetails = null;
        public View.NodeMap constellation = null;
        public View.SkillSpecializer specializer = null;
        public Button backButton = null;
        public View.ChampionHeadline championHeadline = null;

        private Model.ConstellationPreset preset = null;
        private Model.HoveredSkill hovered = new Model.HoveredSkill();

        void Start()
        {
            Debug.Assert(nodeTextualDetails != null);
            Debug.Assert(constellation != null);
            Debug.Assert(specializer != null);
            Debug.Assert(backButton != null);
            Debug.Assert(championHeadline != null);

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
            preset = new Model.ConstellationPreset(new SimpleJSON.JSONObject(), new Model.PresetLimits(1, 1, 1));
            preset.PresetUpdated += OnPresetUpdated;
            var constellationModel = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
            List<Model.Skill> filteredSkillList = new List<Model.Skill>();
            foreach (var node in constellationModel.AbilityNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var node in constellationModel.KitNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var skill in App.Content.Account.ActiveChampion.ClassPreset.SelectedClassList)
                filteredSkillList.Add(skill);

            //view
            backButton.onClick.AddListener(() => { App.Scene.Load("Landing"); });
            nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
            constellation.SetContext(new ViewModel.NodeMapConstellation(
                constellationModel,
                preset,
                filteredSkillList,
                hovered));
            championHeadline.Setup();
        }

        void OnPresetUpdated()
        {
            Model.Skill selectedSkill = null;
            if (preset.SelectedAbilityList.Count > 0)
                selectedSkill = preset.SelectedAbilityList[0];
            else if (preset.SelectedClassList.Count > 0)
                selectedSkill = preset.SelectedClassList[0];
            else if (preset.SelectedKitList.Count > 0)
                selectedSkill = preset.SelectedKitList[0];

            App.Scene.Load("Specialize");
        }
    }
}
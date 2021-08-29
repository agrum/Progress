﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts.Scene
{
    class SpecializeOverview : MonoBehaviour
    {
        public View.NodeTextualDetails nodeTextualDetails = null;
        public View.NodeMap constellation = null;
        public View.SkillSpecializer specializer = null;
        public Button backButton = null;
        public View.ChampionHeadline championHeadline = null;
        public View.WestText sceneTitle = null;

        private Model.ConstellationPreset preset = null;
        private Model.HoveredSkill hovered = new Model.HoveredSkill();

        IEnumerator Start()
        {
            Debug.Assert(nodeTextualDetails != null);
            Debug.Assert(constellation != null);
            Debug.Assert(specializer != null);
            Debug.Assert(backButton != null);
            Debug.Assert(championHeadline != null);
            Debug.Assert(sceneTitle != null);

            yield return App.Content.Account.ActiveChampion?.Load();
            
            if (this == null)
                yield break;

            //model
            preset = new Model.ConstellationPreset(new SimpleJSON.JSONObject(), new Model.PresetLimits(1, 1, 1));
            preset.PresetUpdated += OnPresetUpdated;
            var constellationModel = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
            List<Data.Skill.Skill> filteredSkillList = new List<Data.Skill.Skill>();
            foreach (var node in constellationModel.AbilityNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var node in constellationModel.KitNodeList)
                filteredSkillList.Add(node.Skill);
            foreach (var skill in App.Content.Account.ActiveChampion.ClassPreset.SelectedClassList)
                filteredSkillList.Add(skill);
            hovered.ChangedEvent += OnHoveredChanged;

            //view
            backButton.onClick.AddListener(() => { App.Scene.Load("Landing"); });
            nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));
            constellation.SetContext(new ViewModel.NodeMapConstellation(
                constellationModel,
                preset,
                filteredSkillList,
                hovered));
            yield return championHeadline.Start();
            sceneTitle.Format(App.Content.Account.ActiveChampion.Json["specPointsRemaining"]);
            OnHoveredChanged();
        }
        
        private void OnDestroy()
        {
            hovered.ChangedEvent -= OnHoveredChanged;
        }

        void OnPresetUpdated()
        {
            Specialize.SelectedSkill = null;
            if (preset.SelectedAbilityList.Count > 0)
                Specialize.SelectedSkill = preset.SelectedAbilityList[0];
            else if (preset.SelectedClassList.Count > 0)
                Specialize.SelectedSkill = preset.SelectedClassList[0];
            else if (preset.SelectedKitList.Count > 0)
                Specialize.SelectedSkill = preset.SelectedKitList[0];

            App.Scene.Load("Specialize");
        }

        void OnHoveredChanged()
        {
            specializer.gameObject.SetActive(hovered.Skill != null);
            if (hovered.Skill != null)
            {
                var skillUpgrade = App.Content.Account.ActiveChampion.Upgrades[hovered.Skill];
                specializer.SetContext(
                    new ViewModel.SkillSpecializer(
                        new Model.SkillSpecializer(App.Content.Account.ActiveChampion, skillUpgrade), 
                        skillUpgrade, 
                        false, 
                        hovered));
            }
        }
    }
}
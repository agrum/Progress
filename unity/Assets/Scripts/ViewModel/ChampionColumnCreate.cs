using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
    class ChampionColumnCreate : IBase
    {
        public Model.ConstellationPreset preset;
        public Model.HoveredSkill hovered;

        private string name;

        public ChampionColumnCreate(
            Model.ConstellationPreset model_,
            Model.HoveredSkill hovered_)
        {
            preset = model_;
            hovered = hovered_;
        }

        public INodeMap CreatePreviewContext()
        {
            return new NodeMapPreset(
                preset,
                hovered,
                true);
        }

        public void CreateClicked()
        {
            if (preset.SelectedClassList.Count < 3)
            {
                App.Resource.Prefab.Popup().Setup(
                    "Champion creation error",
                    "3 classes must be selected before continuing.");
                return;
            }

            if (name == null || name.Length < 3)
            {
                App.Resource.Prefab.Popup().Setup(
                    "Champion creation error",
                    "Name must be at least 3 characters long.");
                return;
            }

            JSONArray classes = new JSONArray();
            foreach (var skill in preset.SelectedClassList)
                classes.Add(skill._Id.ToString());

            App.Content.Account.AddChampion(name, classes);
        }

        public void NameChanged(string name_)
        {
            name = name_;
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;

namespace West.ViewModel
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
                return;

            JSONArray classes = new JSONArray();
            foreach (var skill in preset.SelectedClassList)
                classes.Add(skill.Json["_id"]);

            App.Content.Account.AddChampion(name, classes);
        }

        public void NameChanged(string name_)
        {
            name = name_;
        }
    }
}
using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.ViewModel
{
    public class ChampionColumnPreview : IBase
    {
        public event OnVoidDelegate ChampionDestroyed = delegate { };

        private Model.Champion champion = null;
        private Model.HoveredSkill hovered = null;
        private bool canEdit = false;

        public ChampionColumnPreview(
            Model.Champion champion_,
            Model.HoveredSkill hovered_,
            bool canEdit_)
        {
            Debug.Assert(champion_ != null);

            champion = champion_;
            hovered = hovered_;
            canEdit = canEdit_;

            App.Content.Account.ChampionRemoved += OnChampionRemoved;
        }

        public string Name()
        {
            return champion.Json["name"];
        }

        public int Level()
        {
            return champion.Json["level"];
        }

        public int Gear()
        {
            return champion.Json["gear"];
        }

        public INodeMap CreateClassContext()
        {
            return new NodeMapPreset(champion.ClassPreset, hovered, canEdit);
        }

        public void DeleteClicked()
        {
            App.Content.Account.RemoveChampion(champion);
        }

        public void ProceedClicked()
        {
            App.Content.Account.ActivateChampion(champion);
            App.Scene.Load("Landing");
        }

        private void OnChampionRemoved(Model.Champion champion_)
        {
            if (champion == champion_)
            {
                ChampionDestroyed();
            }
        }
    }
}

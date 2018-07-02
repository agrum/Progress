using UnityEngine;

namespace Assets.Scripts.Model.Resource
{
	public class Prefab
    {
        private GameObject constellationNode = null;
        private GameObject loadingCanvas = null;
        private GameObject popup = null;
        private GameObject specializerField = null;

        public Prefab()
        {
            constellationNode = Resources.Load("Prefabs/ConstellationNode") as GameObject;
            loadingCanvas = Resources.Load("Prefabs/LoadingCanvas") as GameObject;
            popup = Resources.Load("Prefabs/UI/Popup") as GameObject;
            specializerField = Resources.Load("Prefabs/UI/SpecializationField") as GameObject;
        }

        public View.Node Node()
        {
            return GameObject.Instantiate(App.Resource.Prefab.constellationNode).GetComponent<View.Node>();
        }

        public GameObject LoadingCanvas()
        {
            return GameObject.Instantiate(App.Resource.Prefab.loadingCanvas);
        }

        public View.Popup Popup()
        {
            return GameObject.Instantiate(App.Resource.Prefab.popup).GetComponent<View.Popup>();
        }

        public View.SkillSpecializationField SkillSpecializerField()
        {
            return GameObject.Instantiate(App.Resource.Prefab.specializerField).GetComponent<View.SkillSpecializationField>();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    class Landing : MonoBehaviour
	{
		public View.TextButton buttonPlayV = null;
		public View.TextButton buttonSpecializeV = null;
        public View.TextButton buttonGearV = null;
        public View.TextButton buttonTradeV = null;
        public Button backButton = null;
        public View.ChampionHeadline championHeadline = null;

        void Start()
		{
			Debug.Assert(buttonPlayV != null);
			Debug.Assert(buttonSpecializeV != null);
			Debug.Assert(buttonGearV != null);
            Debug.Assert(buttonTradeV != null);
            Debug.Assert(championHeadline != null);

            var loadingScreen = App.Resource.Prefab.LoadingCanvas();

            App.Content.Account.Load(() =>
            {
                Destroy(loadingScreen);

                Setup();
            });
        }

        private void Setup()
        {
            if (this == null)
                return;

            buttonPlayV.clickEvent += () => { App.Scene.Load("PresetSelection"); };
            buttonSpecializeV.clickEvent += () => { App.Scene.Load("SpecializeOverview"); };
            backButton.onClick.AddListener(BackClicked);
            championHeadline.Setup();
        }
            
        private void BackClicked()
        {
            App.Scene.Load("ChampionSelection");
        }
    }
}

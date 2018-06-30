using UnityEngine;
using UnityEngine.UI;

namespace West
{
    namespace Scene
    {
        class Landing : MonoBehaviour
		{
			public Canvas canvas = null;
			public View.TextButton buttonPlayV = null;
			public View.TextButton buttonSpecializeV = null;
            public View.TextButton buttonGearV = null;
            public View.TextButton buttonTradeV = null;
            public Button backButton = null;
            public Text ExplorerName = null;
            public Text ExplorerLevel = null;
            public Text ExplorerGear = null;

            void Start()
			{
				Debug.Assert(canvas != null);
				Debug.Assert(buttonPlayV != null);
				Debug.Assert(buttonSpecializeV != null);
				Debug.Assert(buttonGearV != null);
				Debug.Assert(buttonTradeV != null);
				
                canvas.gameObject.SetActive(false);

                App.Content.Account.Load(() =>
                {
                    Setup();
                });
            }

            private void Setup()
            {
                if (this == null)
                    return;
				
				buttonPlayV.clickEvent += () => { App.Scene.Load("PresetSelection"); };
                backButton.onClick.AddListener(BackClicked);

                canvas.gameObject.SetActive(true);
            }
            
            private void BackClicked()
            {
                App.Scene.Load("ChampionSelection");
            }
        }
    }
}

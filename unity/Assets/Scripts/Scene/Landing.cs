using System.Collections;
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

        IEnumerator Start()
		{
			Debug.Assert(buttonPlayV != null);
			Debug.Assert(buttonSpecializeV != null);
			Debug.Assert(buttonGearV != null);
            Debug.Assert(buttonTradeV != null);
            Debug.Assert(championHeadline != null);
            
            yield return StartCoroutine(App.Content.Account.Load());

            if (this == null)
                yield break; 

            buttonPlayV.clickEvent += () => { App.Scene.Load("PresetSelection"); };
            buttonSpecializeV.clickEvent += () => { App.Scene.Load("SpecializeOverview"); };
            backButton.onClick.AddListener(BackClicked);

            yield return championHeadline.Start();
        }
            
        private void BackClicked()
        {
            App.Scene.Load("ChampionSelection");
        }
    }
}

using UnityEngine;

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

				canvas.gameObject.SetActive(true);
			}
        }
    }
}

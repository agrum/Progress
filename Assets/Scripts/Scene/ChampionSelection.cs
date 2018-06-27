using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

namespace West
{
	namespace Scene
	{
		class ChampionSelection : MonoBehaviour
        {
            public View.NodeTextualDetails nodeTextualDetails = null;
            public Canvas canvas = null;
			public RectTransform contentElement = null;
			public HorizontalLayoutGroup horizontalLayout = null;
            public GameObject championColumnPrefab = null;
            public GameObject additionColumnPrefab = null;

            private Model.HoveredSkill hovered = new Model.HoveredSkill();

			void Start()
            {
                Debug.Assert(nodeTextualDetails != null);
                Debug.Assert(canvas != null);
                Debug.Assert(horizontalLayout != null);
                Debug.Assert(championColumnPrefab != null);
                Debug.Assert(additionColumnPrefab != null);

                canvas.gameObject.SetActive(false);

				App.Content.Account.Load(() =>
				{
					Setup();
				});
			}

			private void Setup()
			{
				//return if object died while waiting for answer
				if (this == null)
					return;

                //deactivate any champion
                App.Content.Account.ActivateChampion(null);

				//setup existing preset columns
				foreach (var champion in App.Content.Account.ChampionList)
				{
					GameObject gob = Instantiate(championColumnPrefab);
					gob.transform.SetParent(horizontalLayout.transform, false);
					View.ChampionColumnPreview viewChampionColumn = gob.GetComponent<View.ChampionColumnPreview>();

                    viewChampionColumn.SetContext(new ViewModel.ChampionColumnPreview(champion, hovered, false));
				}

				//add empty column to add presets.
				{
					GameObject gob = Instantiate(additionColumnPrefab);
					gob.transform.SetParent(horizontalLayout.transform, false);
                    View.TextButton addBtn = gob.GetComponentInChildren<View.TextButton>();

                    addBtn.clickEvent += OnAddClicked;
				}

                nodeTextualDetails.SetContext(new ViewModel.NodeTextualDetails(hovered));

                ArrangeUI();
                
				App.Content.Account.ChampionRemoved += OnChampionRemoved;

				canvas.gameObject.SetActive(true);
			}

			void OnDestroy()
			{
				App.Content.Account.ChampionRemoved -= OnChampionRemoved;
			}

			private void OnChampionRemoved(Model.Champion champion)
			{
				ArrangeUI();
			}

			private void ArrangeUI()
            {
                contentElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150.0f * (App.Content.Account.ChampionList.Count + 1));
            }

            private void OnAddClicked()
            {
                App.Scene.Load("ChampionCreator");
            }
        }
	}
}

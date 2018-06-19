using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace West
{
	namespace Scene
	{
		class ChampionSelection : MonoBehaviour
		{
			public Canvas canvas = null;
			public RectTransform contentElement = null;
			public HorizontalLayoutGroup horizontalLayout = null;
            public GameObject championColumnPrefab = null;
            public GameObject additionColumnPrefab = null;

            private Model.HoveredSkill hovered = new Model.HoveredSkill();

			void Start()
			{
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
					View.ChampionColumn viewChampionColumn = gob.GetComponent<View.ChampionColumn>();

                    viewChampionColumn.SetContext(new ViewModel.ChampionColumn(champion, hovered, false));
				}

				//add empty column to add presets.
				{
					GameObject gob = Instantiate(additionColumnPrefab);
					gob.transform.SetParent(horizontalLayout.transform, false);
					Button addBtn = gob.GetComponentInChildren<Button>();

                    addBtn.onClick.AddListener(() =>
                    {
                        GameObject.Instantiate(Resources.Load("Prefabs/LoadingCanvas", typeof(GameObject)));
                        SceneManager.LoadScene("ChampionCreator");
                    });
				}

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
        }
	}
}

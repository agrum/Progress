using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;
using System.Collections;

namespace Assets.Scripts.Scene
{
	class ChampionSelection : MonoBehaviour
    {
        public View.NodeTextualDetails nodeTextualDetails = null;
		public RectTransform contentElement = null;
		public HorizontalLayoutGroup horizontalLayout = null;
        public GameObject championColumnPrefab = null;
        public GameObject additionColumnPrefab = null;

        private Model.HoveredSkill hovered = new Model.HoveredSkill();

		IEnumerator Start()
        {
            Debug.Assert(nodeTextualDetails != null);
            Debug.Assert(horizontalLayout != null);
            Debug.Assert(championColumnPrefab != null);
            Debug.Assert(additionColumnPrefab != null);
            
            yield return StartCoroutine(App.Content.Account.Load());

			//return if object died while waiting for answer
			if (this == null)
				yield break;

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

using UnityEngine;
using UnityEngine.UI;

namespace West
{
	namespace View
	{
		class ChampionColumn : MonoBehaviour
		{
			public NodeMap classPreview = null;
			public Button deleteButton = null;
			public Button proceedButton = null;
            public Text nameText = null;
            public Text levelText = null;
            public Text gearText = null;

            private ViewModel.ChampionColumn viewModel;

			public void SetContext(ViewModel.ChampionColumn viewModel_)
			{
				Debug.Assert(viewModel_ != null);

				viewModel = viewModel_;
				viewModel.ChampionDestroyed += OnChampionDestroyed;

                classPreview.SetContext(viewModel.CreateClassContext());
			}

            private void Start()
			{
				Debug.Assert(classPreview != null);
				Debug.Assert(deleteButton != null);
				Debug.Assert(proceedButton != null);
				Debug.Assert(nameText != null);
                Debug.Assert(levelText != null);
                Debug.Assert(gearText != null);

                DisableAll();
                
				deleteButton.onClick.AddListener(viewModel.DeleteClicked);
				proceedButton.onClick.AddListener(viewModel.ProceedClicked);

                nameText.text = viewModel.Name();
                levelText.text = viewModel.Level().ToString();
                gearText.text = viewModel.Gear().ToString();
            }
			
			private void OnChampionDestroyed()
			{
				Destroy(gameObject);
			}

			private void OnDestroy()
            {
                if (viewModel == null)
                    return;

                viewModel.ChampionDestroyed -= OnChampionDestroyed;
				viewModel = null;
			}

			private void DisableAll()
			{
				deleteButton.gameObject.SetActive(false);
				proceedButton.gameObject.SetActive(false);
                nameText.gameObject.SetActive(false);
                levelText.gameObject.SetActive(false);
                gearText.gameObject.SetActive(false);
            }
		}
	}
}

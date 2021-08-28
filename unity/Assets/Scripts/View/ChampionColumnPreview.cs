using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	class ChampionColumnPreview : MonoBehaviour
	{
		public NodeMap classPreview = null;
		public Button deleteButton = null;
		public Button proceedButton = null;
        public Text nameText = null;
        public Text levelText = null;
        public Text gearText = null;

        private ViewModel.ChampionColumnPreview viewModel;

		public void SetContext(ViewModel.ChampionColumnPreview viewModel_)
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

            EnableAll(true);

			deleteButton.onClick.AddListener(viewModel.DeleteClicked);
			proceedButton.onClick.AddListener(viewModel.ProceedClicked);

            nameText.text = viewModel.Name();
            levelText.text += ": " + viewModel.Level().ToString();
            gearText.text += ": " + viewModel.Gear().ToString();
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

		private void EnableAll(bool enable)
		{
			deleteButton.gameObject.SetActive(enable);
			proceedButton.gameObject.SetActive(enable);
            nameText.gameObject.SetActive(enable);
            levelText.gameObject.SetActive(enable);
            gearText.gameObject.SetActive(enable);
        }
	}
}

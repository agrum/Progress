using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    class ChampionColumnCreate : MonoBehaviour
    {
        public NodeMap presetPreview = null;
        public Button createButton = null;
        public InputField nameInput = null;

        private ViewModel.ChampionColumnCreate viewModel;

        public void SetContext(ViewModel.ChampionColumnCreate viewModel_)
        {
            Debug.Assert(viewModel_ != null);

            viewModel = viewModel_;

            presetPreview.SetContext(viewModel.CreatePreviewContext());
        }

        IEnumerator Start()
        {
            Debug.Assert(presetPreview != null);
            Debug.Assert(createButton != null);
            Debug.Assert(nameInput != null);

            while (viewModel == null)
                yield return null;

            createButton.onClick.AddListener(viewModel.CreateClicked);
            nameInput.onEndEdit.AddListener(viewModel.NameChanged);
        }

        private void OnDestroy()
        {
            if (viewModel == null)
                return;
                
            viewModel = null;
        }
    }
}

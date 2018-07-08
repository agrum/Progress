using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace Assets.Scripts.View
{
    class SkillSpecializer : MonoBehaviour
    {
        public Text nodeName = null;
        public Node node = null;
        public WestText handicap = null;
        public Transform fieldsContainer = null;
        public ProgressSlider overallWeight = null;

        private ViewModel.SkillSpecializer viewModel = null;
        private float preferredFieldHeight = 0;

        public void SetContext(ViewModel.SkillSpecializer viewModel_)
        {
            Debug.Assert(fieldsContainer != null);
            Debug.Assert(viewModel_ != null);

            if (viewModel != null)
                OnDestroy();

            viewModel = viewModel_;
            viewModel.NodeAdded += OnNodeAdded;
            viewModel.SpecializerFieldAdded += OnSpecializerFieldAdded;
            viewModel.SkillUpgraded += OnSkillUpgraded;

            nodeName.text = viewModel.Name();
            OnSkillUpgraded();

            while (fieldsContainer.childCount > 0)
                GameObject.DestroyImmediate(fieldsContainer.GetChild(0).gameObject);
            viewModel.Setup();
        }

        private void OnDestroy()
        {
            if (viewModel != null)
            { 
                viewModel.NodeAdded -= OnNodeAdded;
                viewModel.SpecializerFieldAdded -= OnSpecializerFieldAdded;
                viewModel.SkillUpgraded -= OnSkillUpgraded;
                viewModel = null;
            }
        }

        private void Update()
        {
            fieldsContainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredFieldHeight * (fieldsContainer.childCount));
        }

        private void OnNodeAdded(ViewModel.Factory factory)
        {
            node.SetContext(factory() as ViewModel.INode);
            node.OnScaleChanged(80.0f);
        }

        private void OnSpecializerFieldAdded(ViewModel.Factory factory)
        {
            SkillSpecializationField field = App.Resource.Prefab.SkillSpecializerField();
            preferredFieldHeight = field.GetComponent<RectTransform>().rect.height;
            field.SetContext(factory() as ViewModel.SkillSpecializationField);
            field.transform.SetParent(fieldsContainer, false);
        }

        private void OnSkillUpgraded()
        {
            handicap.Format(viewModel.PreviewHandicap().ToString());
            overallWeight.Main = viewModel.OverallWeight();
            overallWeight.Progress = viewModel.OverallPreviewWeight();
        }
    }
}

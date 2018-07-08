using UnityEngine;

namespace Assets.Scripts.View
{
    public class SkillSpecializationField : MonoBehaviour
    {
        public WestText fieldName = null;
        public WestText fieldPercentage = null;
        public TextButton plusButton = null;
        public TextButton minusButton = null;
        public ProgressSlider plusSlider = null;
        public ProgressSlider minusSlider = null;

        private ViewModel.SkillSpecializationField viewModel = null;

        public void SetContext(ViewModel.SkillSpecializationField viewModel_)
        {
            Debug.Assert(viewModel_ != null);

            viewModel = viewModel_;
            viewModel.LevelChanged += SetupUI;

            plusButton.enterEvent += () => viewModel.PreviewUpgrade(true);
            minusButton.enterEvent += () => viewModel.PreviewDowngrade(true);
            plusButton.leaveEvent += () => viewModel.PreviewUpgrade(false);
            minusButton.leaveEvent += () => viewModel.PreviewDowngrade(false);
            plusButton.clickEvent += () => viewModel.Upgrade();
            minusButton.clickEvent += () => viewModel.Downgrade();

            fieldName.Format(viewModel.Category(), viewModel.Name());

            SetupUI();
        }

        void OnDestroy()
        {
            if (viewModel != null)
            {
                viewModel.LevelChanged -= SetupUI;
                viewModel = null;
            }
        }

        void SetupUI()
        { 
            plusButton.gameObject.SetActive(viewModel.Editable());
            minusButton.gameObject.SetActive(viewModel.Editable());
            plusSlider.gameObject.SetActive(true);
            minusSlider.gameObject.SetActive(true);

            if (viewModel.Sign() == Model.MetricUpgrade.SpecializeSign.Positive)
            {
                minusSlider.gameObject.SetActive(false);
            }
            else if (viewModel.Sign() == Model.MetricUpgrade.SpecializeSign.Negative)
            {
                plusSlider.gameObject.SetActive(false);
            }

            plusSlider.Main = System.Math.Abs(viewModel.Level());
            minusSlider.Main = System.Math.Abs(viewModel.Level());
            plusSlider.Progress = System.Math.Abs(viewModel.TemporaryLevel());
            minusSlider.Progress = System.Math.Abs(viewModel.TemporaryLevel());
            
            fieldPercentage.color = viewModel.TemporaryLevel() >= 0
                ? App.Resource.Material.AbilityMaterial.color
                : App.Resource.Material.KitMaterial.color;
            var temporaryFactor = viewModel.TemporaryFactor();
            fieldPercentage.Format(
                temporaryFactor >= 0 ? "+" : "",
                temporaryFactor.ToString());
        }
    }
}

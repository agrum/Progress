using UnityEngine;

namespace Assets.Scripts.View
{
    public class SkillSpecializationField : MonoBehaviour
    {
        public TextButton plusButton = null;
        public TextButton minusButton = null;
        public ProgressSlider plusSlider = null;
        public ProgressSlider minusSlider = null;

        private ViewModel.SkillSpecializationField viewModel = null;

        public void SetContext(ViewModel.SkillSpecializationField viewModel_)
        {
            Debug.Assert(viewModel_ != null);

            viewModel = viewModel_;
            viewModel.SpecLevelChanged += SetupUI;

            plusButton.enterEvent += ShowPositiveProgress;
            minusButton.enterEvent += ShowNegativeProgress;
            plusButton.leaveEvent += HidePositiveProgress;
            minusButton.leaveEvent += HideNegativeProgress;
            plusButton.clickEvent += () => viewModel.Buy(Model.MetricUpgrade.SpecializeSign.Positive);
            minusButton.clickEvent += () => viewModel.Buy(Model.MetricUpgrade.SpecializeSign.Negative);

            SetupUI();
        }

        void SetupUI()
        { 
            plusButton.gameObject.SetActive(viewModel.Editable());
            minusButton.gameObject.SetActive(viewModel.Editable());
            plusSlider.gameObject.SetActive(false);
            minusSlider.gameObject.SetActive(false);

            if (viewModel.Sign() == Model.MetricUpgrade.SpecializeSign.Positive)
            {
                plusSlider.gameObject.SetActive(true);
            }
            else if (viewModel.Sign() == Model.MetricUpgrade.SpecializeSign.Negative)
            {
                minusSlider.gameObject.SetActive(true);
            }

            plusSlider.Main = viewModel.SpecLevel();
            minusSlider.Main = viewModel.SpecLevel();
        }

        void ShowPositiveProgress()
        {
            plusSlider.gameObject.SetActive(true);
            plusSlider.Progress = viewModel.NextSpecLevel(Model.MetricUpgrade.SpecializeSign.Positive);
        }

        void ShowNegativeProgress()
        {
            minusSlider.gameObject.SetActive(true);
            minusSlider.Progress = viewModel.NextSpecLevel(Model.MetricUpgrade.SpecializeSign.Negative);
        }

        void HidePositiveProgress()
        {
            plusSlider.Progress = 0;
            if (viewModel.Sign() == Model.MetricUpgrade.SpecializeSign.None)
                plusSlider.gameObject.SetActive(false);
        }

        void HideNegativeProgress()
        {
            minusSlider.Progress = 0;
            if (viewModel.Sign() == Model.MetricUpgrade.SpecializeSign.None)
                minusSlider.gameObject.SetActive(false);
        }
    }
}

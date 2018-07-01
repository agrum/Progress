using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace Assets.Scripts.View
{
    class SkillSpecializer : MonoBehaviour
    {
        public Text nodeName = null;
        public NodeMap nodeMap = null;
        public WestText handicap = null;

        private ViewModel.SkillSpecializer viewModel = null;

        public void SetContext(ViewModel.SkillSpecializer viewModel_)
        {
            Debug.Assert(viewModel_ != null);

            viewModel = viewModel_;
            viewModel.SkillChanged += OnSkillChanged;
        }

        private void OnSkillChanged(JSONNode skill)
        {

        }
    }
}

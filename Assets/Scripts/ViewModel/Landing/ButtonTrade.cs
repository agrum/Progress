using UnityEngine;

namespace West
{
    namespace ViewModel
    {
        public class ButtonTrade : MonoBehaviour
        {
            View.TextButton btn = null;

            public ButtonTrade(View.TextButton button)
            {
                btn = button;
                btn.SetText("Trade");
                btn.clickEvent += OnClick;
            }

            private void OnClick()
            {

            }
        }
    }
}

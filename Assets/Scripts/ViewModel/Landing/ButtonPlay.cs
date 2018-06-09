using UnityEngine;

namespace West
{
    namespace ViewModel
    {
        public class ButtonPlay : MonoBehaviour
        {
            View.TextButton btn = null;

            public ButtonPlay(View.TextButton button)
            {
                btn = button;
                btn.SetText("Play");
                btn.clickEvent += OnClick;
            }

            private void OnClick()
            {

            }
        }
    }
}

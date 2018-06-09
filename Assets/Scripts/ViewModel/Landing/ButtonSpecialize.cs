using UnityEngine;

namespace West
{
    namespace ViewModel
    {
        public class ButtonSpecialize : MonoBehaviour
        {
            View.TextButton btn = null;

            public ButtonSpecialize(View.TextButton button)
            {
                btn = button;
                btn.SetText("Specialize");
                btn.clickEvent += OnClick;
            }

            private void OnClick()
            {

            }
        }
    }
}

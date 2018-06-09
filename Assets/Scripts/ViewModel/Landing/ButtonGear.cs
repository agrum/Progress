using UnityEngine;

namespace West
{
    namespace ViewModel
    {
        public class ButtonGear : MonoBehaviour
        {
            View.TextButton btn = null;

            public ButtonGear(View.TextButton button)
            {
                btn = button;
                btn.SetText("Gear");
                btn.clickEvent += OnClick;
            }

            private void OnClick()
            {
                
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace West
{
    namespace View
    {
        public class TextButton : MonoBehaviour
        {
            public delegate void OnClickDelegate();
            public event OnClickDelegate clickEvent;

            public void Start()
            {
                Button btn = GetComponent<Button>();
                btn.onClick.AddListener(OnClick);
            }

            public void SetText(string text)
            {
                Text txt = GetComponentInChildren<Text>();
                txt.text = text;
            }

            private void OnClick()
            {
                clickEvent();
            }
        }
    }
}

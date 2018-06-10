using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace West
{
    namespace View
    {
        public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
        {
            public delegate void OnClickDelegate();
            public event OnClickDelegate clickEvent = delegate { };

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
            public void OnPointerEnter(PointerEventData pointerEventData)
            {
                Image image = GetComponent<Image>();
                image.color = new Color(0.9f, 0.9f, 0.9f);
            }
            
            public void OnPointerExit(PointerEventData pointerEventData)
            {
                Image image = GetComponent<Image>();
                image.color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
    }
}

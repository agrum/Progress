using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{ 
    public class Popup : WestBehaviour
    {
        public Text title;
        public Text message;
        public Button exitButton;

        public void Setup(string title_, string message_)
        {
            Delay(() =>
            {
                title.text = title_;
                message.text = message_;

                exitButton.onClick.AddListener(() => 
                {
                    Destroy(gameObject);
                });
            });
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{ 
    public class Popup : MonoBehaviour
    {
        public bool isRunning = false;
        public Text title;
        public Text message;
        public Button exitButton;

        public IEnumerator Setup(string title_, string message_)
        {
            while (!isRunning)
            {
                yield return null;
            }

            title.text = title_;
            message.text = message_;

            exitButton.onClick.AddListener(() => 
            {
                Destroy(gameObject);
            });
        }

        void Start()
        {
            isRunning = true;
        }
    }
}

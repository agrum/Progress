using BestHTTP;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class WorldUpdate : MonoBehaviour
    {
        public GameObject debugButtonPrefab = null;
        public HorizontalLayoutGroup horizontalLayout = null;
        public GameObject templatesContainer = null;

        public delegate void OnClicked();

        // Use this for initialization
        IEnumerator Start()
        {
            Debug.Assert(debugButtonPrefab != null);
            Debug.Assert(horizontalLayout != null);

            yield return StartCoroutine(App.Content.Session.Load());

            foreach (var world in templatesContainer.GetComponentsInChildren<West.Asset.World.Generator>())
            {
                AddButton(string.Format("Update world \"{0}\"", world.gameObject.name), world.Export);
            }
        }

        void AddButton(string name_, UnityEngine.Events.UnityAction delegate_)
        {
            GameObject dataUpdateButton = Instantiate(debugButtonPrefab);
            dataUpdateButton.transform.SetParent(horizontalLayout.transform, false);
            Button button = dataUpdateButton.transform.Find("Button").gameObject.GetComponent<Button>();
            button.onClick.AddListener(delegate_);
            Text text = button.transform.Find("Text").gameObject.GetComponent<Text>();
            text.text = name_;
        }
    }
}

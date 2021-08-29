using BestHTTP;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class DataUpdate : MonoBehaviour
    {
        public GameObject debugButtonPrefab = null;
        public HorizontalLayoutGroup horizontalLayout = null;
        Utility.Scheduler scheduler;
        
        public delegate void OnClicked();

        // Use this for initialization
        IEnumerator Start()
        {
            Debug.Assert(debugButtonPrefab != null);
            Debug.Assert(horizontalLayout != null);
            scheduler = new Utility.Scheduler(this);

            yield return StartCoroutine(App.Content.Session.Load());

            AddButton("Update skills", SkillUpdate);
            AddButton("Test stackers", StackerUnitTest);
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

        void SkillUpdate()
        {
            var testSkill = Asset.SkillExport.Exporter.LaserBeam();
            Debug.Log(testSkill);
            var request = App.Server.Request(
                HTTPMethods.Post,
                "tools/skillsUpdate",
                (JSONNode json_) =>
                {
                    Debug.Log(json_.ToString());
                });
            request.AddHeader("Content-Type", "application/json");
            request.RawData = System.Text.Encoding.UTF8.GetBytes(testSkill);
            request.Send();
        }

        void StackerUnitTest()
        {
            StartCoroutine(Context.Skill.Stacker.Base.UnitTest(scheduler));
        }
    }
}

using BestHTTP;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class EmptyWorld : MonoBehaviour
    {
        public Context.PlayerNew player;

        // Use this for initialization
        IEnumerator Start()
        {
            yield return StartCoroutine(App.Content.SkillList.Load());
        }
    }
}

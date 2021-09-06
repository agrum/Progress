using BestHTTP;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class OutdoorSession : MonoBehaviour
    {
        public Context.PlayerNew player;
        public string layoutId;
        public int size;

        // Use this for initialization
        IEnumerator Start()
        {
            yield return StartCoroutine(App.Content.Layouts.Load());
        }
    }
}

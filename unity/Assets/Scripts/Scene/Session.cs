using BestHTTP;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
    public class Session : MonoBehaviour
    {
        public Context.PlayerNew Player;

        protected bool loaded = false;

        public IEnumerator Load()
        {
            while (!loaded)
            {
                yield return new WaitForSeconds(0);
            }
        }
    }
}

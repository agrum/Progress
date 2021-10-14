using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data.Layout
{
    public class DecorLibrary : MonoBehaviour
    {
        public Environment.EVariety Variety;
        public float Density;
        public List<GameObject> Decors = new List<GameObject>();

        // Use this for initialization
        void Start()
        {

        }

        public GameObject Populate()
        {
            if (UnityEngine.Random.value < Density)
            {
                var decor = Instantiate(Decors[UnityEngine.Random.Range(0, Decors.Count)]);
                var scale = UnityEngine.Random.value * 0.3f + 1.0f;
                decor.transform.parent = transform.parent;
                decor.transform.localPosition = transform.localPosition + new Vector3(UnityEngine.Random.value - 0.5f, UnityEngine.Random.value - 0.5f, -1.0f);
                decor.transform.localRotation = Quaternion.Euler(UnityEngine.Random.value * 360.0f, -90.0f, 90.0f);
                decor.transform.localScale = new Vector3(scale, scale, scale);
                return decor;
            }
            return null;
        }
    }
}
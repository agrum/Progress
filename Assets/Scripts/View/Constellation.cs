using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace West
{
	namespace View
	{
		public class Constellation : MonoBehaviour
		{
			public delegate void OnSizeChangedDelegate();
			public event OnSizeChangedDelegate SizeChangedEvent = delegate { };

			private RectTransform rectTransform = null;
			private Rect lastRect = new Rect();

			void Start()
			{
				rectTransform = GetComponent<RectTransform>();
			}

			void Update()
			{
				if (rectTransform && rectTransform.rect != lastRect)
				{
					lastRect = rectTransform.rect;
					SizeChangedEvent();
				}
			}

			public GameObject Add(GameObject prefab)
			{
				GameObject gob = GameObject.Instantiate(prefab);
				gob.transform.SetParent(transform);
				return gob;
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace West
{
	namespace Scene
	{
		public class StartupManager : MonoBehaviour
		{
			public Text text;

			// Use this for initialization
			void Start()
			{
				text.text = "Starting up";
				App.Content.GameSettings.Load(() => { });
			}
		}
	}
}
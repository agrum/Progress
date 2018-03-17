using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace West
{
	public class StartupManager : MonoBehaviour
	{
		public Text text;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if(App.Model != null)
			{
				SceneManager.LoadScene("World");
			}
			else if(text != null)
			{
				text.text = "Starting up";
			}
		}
	}
}
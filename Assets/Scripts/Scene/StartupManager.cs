using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Scene
{
	public class StartupManager : MonoBehaviour
	{
		void Start()
		{
			App.Content.GameSettings.Load(() => { });
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine;
using SimpleJSON;
using BestHTTP;

namespace Assets.Scripts
{
	public class App
    {
        static public Scene.Loader Scene { get; private set; } = new Scene.Loader();
        static public Model.Resource.AppResource Resource { get; private set; } = new Model.Resource.AppResource();
		static public CloudContent.AppContent Content { get; private set; } = new CloudContent.AppContent();
        static public Model.Network.Server Server { get; private set; } = new Model.Network.Server();
    }
}
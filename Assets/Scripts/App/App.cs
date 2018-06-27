using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine;
using SimpleJSON;
using BestHTTP;

namespace West
{
	public class App
    {
        static public Model.Loader Scene { get; private set; } = new Model.Loader();
        static public Model.Resource.AppResource Resource { get; private set; } = new Model.Resource.AppResource();
		static public CloudContent.AppContent Content { get; private set; } = new CloudContent.AppContent();
        static public Model.Network.Server Server { get; private set; } = new Model.Network.Server();
    }
}
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
		static public Model.Resource.AppResource Resource { get; private set; } = new Model.Resource.AppResource();
		static public Model.CloudContent.AppContent Content { get; private set; } = new Model.CloudContent.AppContent();
		static public Model.Network.Server Server { get; private set; } = new Model.Network.Server();
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using SimpleJSON;
using BestHTTP;

namespace West
{
	public class App
	{
		static private JSONNode model = null;
		static private HTTPRequest request = null;

		static public JSONNode Model
		{
			get
			{
				if (model == null && request == null)
				{
					//ask server for model base
					request = new HTTPRequest(
						new System.Uri("http://127.0.0.1:3000/gameSettings/Classic"),
						OnRequestFinished);
					request.Send();

					if(SceneManager.GetActiveScene().name != "Startup")
					{
						SceneManager.LoadScene("Startup");
					}
				}
				return model;
			}
		}

		static private void OnRequestFinished(HTTPRequest _request, HTTPResponse response)
		{
			if (request.State == HTTPRequestStates.Finished)
			{
				model = JSON.Parse(request.Response.DataAsText);
			}
			request = null;
		}
	}
}
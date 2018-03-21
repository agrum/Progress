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
		static private string host = "http://127.0.0.1:3000";

		static private JSONNode session = null;
		static private JSONNode model = null;
		static private HTTPRequest request = null;

		static public JSONNode Model
		{
			get
			{
				if (session == null && request == null)
				{
					request = new HTTPRequest(
						new System.Uri(host + "/login"), 
						HTTPMethods.Post,
						OnLoginRequestFinished);
					request.AddField("email", "thomas.lgd@gmail.com");
					request.AddField("password", "plop");
					request.Send();
				}
				else if (model == null && request == null)
				{
					//ask server for model base
					request = new HTTPRequest(
						new System.Uri(host + "/gameSettings/Classic"),
						OnGameSettingsRequestFinished);
					request.Send();
				}
				if(request != null && SceneManager.GetActiveScene().name != "Startup")
				{
					SceneManager.LoadScene("Startup");
				}
				return model;
			}
		}

		static private void OnLoginRequestFinished(HTTPRequest _request, HTTPResponse response)
		{
			if (request.State == HTTPRequestStates.Finished)
			{
				session = JSON.Parse(request.Response.DataAsText);
			}
			request = null;
		}

		static private void OnGameSettingsRequestFinished(HTTPRequest _request, HTTPResponse response)
		{
			if (request.State == HTTPRequestStates.Finished)
			{
				model = JSON.Parse(request.Response.DataAsText);
			}
			request = null;
		}
	}
}
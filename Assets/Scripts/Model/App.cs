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
		static private string bootUpPage = "gameSettings/Classic";
		static private string logInPage = "login";

		static private bool loaded = false;
		static private JSONNode session = null;
		static private JSONNode model = null;
		static private HTTPRequest request = null;
		static private string callbackScene = null;

		public delegate void OnAppLoadedDelegate();
		static private event OnAppLoadedDelegate appLoadedEvent;

		static public void Load(OnAppLoadedDelegate callback)
		{
			if (loaded)
				callback();
			else
			{
				appLoadedEvent += callback;
				if (model == null && request == null)
					RequestGameSettings();
				if (request != null && SceneManager.GetActiveScene().name != "Startup")
				{
					callbackScene = SceneManager.GetActiveScene().name;
					SceneManager.LoadScene("Startup");
				}
			}
		}

		static public JSONNode Model
		{
			get
			{
				return model;
			}
		}
		
		static public System.Uri URI { get; } = new System.Uri(host);

		static private void RequestGameSettings()
		{
			request = new HTTPRequest(
				new System.Uri(URI, bootUpPage),
				OnGameSettingsRequestFinished);
			request.Send();
		}

		static private void OnLoginRequestFinished(HTTPRequest _request, HTTPResponse response)
		{
			if (request.State == HTTPRequestStates.Finished)
			{
				session = JSON.Parse(request.Response.DataAsText);
				RequestGameSettings();
			}
			else
				request = null;
		}

		static private void OnGameSettingsRequestFinished(HTTPRequest _request, HTTPResponse response)
		{
			if (request.State != HTTPRequestStates.Finished)
			{
				request = null;
				return;
			}
			
			var json = JSON.Parse(request.Response.DataAsText);
			if (json["error"] == json["null"])
			{
				model = json;
				loaded = true;
				request = null;
				SceneManager.LoadScene(callbackScene);
				appLoadedEvent();
			}
			else
			{
				Debug.Log(json);
				request = new HTTPRequest(
					new System.Uri(URI, logInPage),
					HTTPMethods.Post,
					OnLoginRequestFinished);
				request.AddField("email", "thomas.lgd@gmail.com");
				request.AddField("password", "plop");
				request.Send();
			}
		}
	}
}
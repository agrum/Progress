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
		static private string abilitiesPage = "ability";
		static private string logInPage = "login";

		static private bool loaded = false;
		static private JSONNode session = null;
		static public JSONNode Model { get; private set; }= null;
		static private HTTPRequest request = null;
		static private string callbackScene = null;

		public delegate void OnAppLoadedDelegate();
		static private event OnAppLoadedDelegate appLoadedEvent;
		
		public delegate void OnAppRespondedDelegate(JSONNode json);

		static public void Load(OnAppLoadedDelegate callback)
		{
			if (loaded)
				callback();
			else
			{
				appLoadedEvent += callback;
				if (Model == null && request == null)
					RequestGameSettings();
				if (request != null && SceneManager.GetActiveScene().name != "Startup")
				{
					callbackScene = SceneManager.GetActiveScene().name;
					SceneManager.LoadScene("Startup");
				}
			}
		}

		static public HTTPRequest Request(HTTPMethods method, string path, OnAppRespondedDelegate callback)
		{
			return Request(method, path, callback, (JSONNode json) => { Debug.Log("Request(+"+ path + ") resulted in \n" + json); });
		}

		static public HTTPRequest Request(HTTPMethods method, string path, OnAppRespondedDelegate callback, OnAppRespondedDelegate err)
		{
			return request = new HTTPRequest(
				new System.Uri(URI, path),
				method,
				(HTTPRequest request_, HTTPResponse response_) =>
				{
					if (request_.State != HTTPRequestStates.Finished)
					{
						request_ = null;
						Debug.Log("Request " + path + " returned null");
						return;
					}

					var json = JSON.Parse(request_.Response.DataAsText);
					if (json != null && json["error"] == json["null"])
						callback(json);
					else
						err(json);
				});
		}
		
		static public System.Uri URI { get; } = new System.Uri(host);

		static private void RequestGameSettings()
		{
			Request(
				HTTPMethods.Get,
				bootUpPage,
				(JSONNode json) => //set game settings if received
				{
					Model = json;
					RequestAbilities();
				},
				(JSONNode json) => //try to log in if not logged in yet
				{
					//Debug.Log(json);
					if (session != null)
						return;

					var request = Request(
						HTTPMethods.Post,
						logInPage,
						(JSONNode json_) => //if log in is successful, try to get game settigns again
						{
							session = json_;
							RequestGameSettings();
						});
					request.AddField("email", "thomas.lgd@gmail.com");
					request.AddField("password", "plop");
					request.Send();
				})
				.Send();
		}

		static private void RequestAbilities()
		{
			Request(
				HTTPMethods.Get,
				abilitiesPage,
				(JSONNode json) => //set abilities if received
				{
					Model["abilities"] = json;
					loaded = true;
					request = null;
					SceneManager.LoadScene(callbackScene);
					appLoadedEvent();
				},
				(JSONNode json) =>
				{
					Debug.Log(json);
				})
				.Send();
		}
	}
}
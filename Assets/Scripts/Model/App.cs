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
		static private string host = "http://127.0.0.1:3000";
		static private string bootUpPage = "gameSettings/Classic";
		static private string abilitiesPage = "ability";
		static private string classesPage = "class";
		static private string kitsPage = "kit";
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

		static public void Async(List<HTTPRequest> request_list, OnAppRespondedDelegate callback)
		{
			JSONNode cumulatedResponses = new JSONArray();
			int request_pending_count = request_list.Count;
			foreach (var request in request_list)
			{
				var oldCallback = request.Callback;
				request.Callback = (HTTPRequest request_, HTTPResponse response_) =>
				{
					//call original callback
					oldCallback(request_, response_);

					//write errors if any.
					if (request_.State != HTTPRequestStates.Finished)
						cumulatedResponses.Add(request_.Uri.ToString() + ": didn't terminate properly");
					var json = JSON.Parse(request_.Response.DataAsText);
					if (!(json != null && json["error"] == json["null"]))
						cumulatedResponses.Add(request_.Uri.ToString() + ": error");

					//decrement counter
					Interlocked.Decrement(ref request_pending_count);

					//when all requests have been processed, call async callback's
					if (Interlocked.Equals(request_pending_count, 0))
						callback(cumulatedResponses);
				};
				request.Send();
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
			var extraRequestList = new List<HTTPRequest>();
			extraRequestList.Add(Request(
				HTTPMethods.Get,
				abilitiesPage,
				(JSONNode json) => { Model["abilities"] = json; },
				(JSONNode json) => { Debug.Log(json); }));
			extraRequestList.Add(Request(
				HTTPMethods.Get,
				classesPage,
				(JSONNode json) => { Model["classes"] = json; },
				(JSONNode json) => { Debug.Log(json); }));
			extraRequestList.Add(Request(
				HTTPMethods.Get,
				kitsPage,
				(JSONNode json) => { Model["kits"] = json; },
				(JSONNode json) => { Debug.Log(json); }));

			Request(
				HTTPMethods.Get,
				bootUpPage,
				(JSONNode json) => //set game settings if received
				{
					Model = json;
					Async(
						extraRequestList,
						(JSONNode json_) =>
						{
							if (json_.AsArray.Count != 0)
							{
								Debug.Log("extraRequestList didn't return properly");
								Debug.Log(json_);
								return;
							}

							loaded = true;
							request = null;
							SceneManager.LoadScene(callbackScene);
							appLoadedEvent();
						});
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
	}
}
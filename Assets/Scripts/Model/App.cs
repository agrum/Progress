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
		static private string bootUpPage = "/gameSettings/Classic";

		static private JSONNode session = null;
		static private JSONNode model = null;
		static private HTTPRequest request = null;

		static public JSONNode Model
		{
			get
			{
				if (model == null && request == null)
				{
					//ask server for model base
					BestHTTP.Statistics.GeneralStatistics stats = HTTPManager.GetGeneralStatistics(BestHTTP.Statistics.StatisticsQueryFlags.All);
					Debug.Log(stats.CookieCount);

					request = new HTTPRequest(
						new System.Uri(host + bootUpPage),
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
			if (request.State != HTTPRequestStates.Finished)
			{
				request = null;
				return;
			}
			
			var json = JSON.Parse(request.Response.DataAsText);
			if (json["error"] == json["null"])
			{
				model = json;
				request = null;
			}
			else
			{
				Debug.Log(json);
				request = new HTTPRequest(
					new System.Uri(host + "/login"),
					HTTPMethods.Post,
					OnLoginRequestFinished);
				request.AddField("email", "thomas.lgd@gmail.com");
				request.AddField("password", "plop");
				request.Send();
			}
		}
	}
}
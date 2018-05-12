using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine;
using SimpleJSON;
using BestHTTP;

namespace West
{
	namespace Model
	{
		namespace Network
		{
			public class Server
			{
				static private string host = "http://127.0.0.1:3000";

				public delegate void OnAppRespondedDelegate(JSONNode json);

				public void Async(List<HTTPRequest> request_list, OnAppRespondedDelegate callback)
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

				public HTTPRequest Request(HTTPMethods method, string path, OnAppRespondedDelegate callback)
				{
					return Request(method, path, callback, (JSONNode json) => { Debug.Log("Request(+" + path + ") resulted in \n" + json); });
				}

				public HTTPRequest Request(HTTPMethods method, string path, OnAppRespondedDelegate callback, OnAppRespondedDelegate err)
				{
					return new HTTPRequest(
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

				public System.Uri URI { get; } = new System.Uri(host);
			}
		}
	}
}
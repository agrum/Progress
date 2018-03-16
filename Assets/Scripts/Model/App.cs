using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using BestHTTP;

namespace West
{
	public class App
	{
		static private JSONNode model = null;

		static public JSONNode Model
		{
			get
			{
				if (model == null)
				{
					//ask server for model base
					HTTPRequest request = new HTTPRequest(
						new System.Uri("http://127.0.0.1:3000/gameSettings/Classic"),
						(HTTPRequest, HTTPResponse) => Debug.Log("gameSettings/Classic acquired"));
					request.Send();

					//wait for server response
					for(int attempts = 0; attempts < 10 && request.State != HTTPRequestStates.Finished; ++attempts)
					{
						if (request.State == HTTPRequestStates.Aborted ||
							request.State == HTTPRequestStates.ConnectionTimedOut ||
							request.State == HTTPRequestStates.Error ||
							request.State == HTTPRequestStates.TimedOut)
						{
							Debug.Log(request.State);
							Application.Quit();
						}
						System.Threading.Thread.Sleep(100);
					}
					model = JSON.Parse(request.Response.DataAsText);
				}
				return model;
			}
		}

	}
}
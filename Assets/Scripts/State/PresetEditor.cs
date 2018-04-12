using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BestHTTP;

namespace West
{
	class PresetEditor : MonoBehaviour
	{
		void Start()
		{
			App.Load(()=>
			{
				var request = new HTTPRequest(
					new System.Uri(App.URI, "constellation"),
					OnConstellationRequestFinished);
				request.Send();
			}); ;
		}

		private void OnConstellationRequestFinished(HTTPRequest request, HTTPResponse response)
		{

		}
	}
}

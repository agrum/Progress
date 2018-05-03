using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine.SceneManagement;
using BestHTTP;

namespace West
{
	namespace Model
	{
		namespace CloudContent
		{
			public class Session : Base
			{
				public JSONNode Json { get; private set; } = null;

				protected override void Build(OnBuilt onBuilt_)
				{
					if (SceneManager.GetActiveScene().name != "Startup")
					{
						callbackScene = SceneManager.GetActiveScene().name;
						SceneManager.LoadScene("Startup");
					}

					App.Request(
					HTTPMethods.Get,
					"gameSettings/Classic",
					(JSONNode json_) =>
					{
						SceneManager.LoadScene(callbackScene);
						Json = json_;
						onBuilt_();
					},
					(JSONNode json) => //try to log in if not logged in yet
					{
						if (Json != null)
							return;

						Json = json;
						var request = App.Request(
							HTTPMethods.Post,
							"login",
							(JSONNode json_) => //if log in is successful, try to get game settigns again
							{
								Json = json;
								Build(onBuilt_);
							});
						request.AddField("email", "thomas.lgd@gmail.com");
						request.AddField("password", "plop");
						request.Send();
					}).Send();
				}

				public Session()
				{

				}

				private string callbackScene = null;
			}
		}
	}
}

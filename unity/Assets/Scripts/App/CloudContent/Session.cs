using SimpleJSON;
using UnityEngine.SceneManagement;
using BestHTTP;
using System.Collections;

namespace Assets.Scripts.CloudContent
{
	public class Session : Base
	{
		public JSONNode Json { get; private set; } = null;
		public string Email { get; private set; } = null;
		public string Username { get; private set; } = null;
		public string Account { get; private set; } = null;

        protected override IEnumerator Build()
        {
            bool built = false;
            bool triedLogin = false;

            while (!built && !triedLogin)
            {
                yield return App.Server.Request(
                HTTPMethods.Get,
                "user",
                (JSONNode json_) =>
                {
                    if (callbackScene != null)
                    {
                        App.Scene.Load(callbackScene);
                    }
                    Json = json_;

                    Email = Json["email"];
                    Username = Json["username"];
                    Account = Json["account"];

                    built = true;
                },
                (JSONNode json) => //try to log in if not logged in yet
                {
                    if (Json != null)
                        return;

                    Json = json;
                    var request = App.Server.Request(
                        HTTPMethods.Post,
                        "login",
                        (JSONNode json_) => //if log in is successful, try to get game settigns again
                        {
                            Json = json;
                            triedLogin = true;
                        });
                    request.AddField("email", "thomas.lgd@gmail.com");
                    request.AddField("password", "plop");
                    request.Send();
                }).Send();
            }
        }

        public Session()
		{

		}

		private string callbackScene = null;
	}
}

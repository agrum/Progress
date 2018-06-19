using SimpleJSON;
using UnityEngine.SceneManagement;
using BestHTTP;

namespace West.CloudContent
{
	public class Session : Base
	{
		public JSONNode Json { get; private set; } = null;
		public string Email { get; private set; } = null;
		public string Username { get; private set; } = null;
		public string Account { get; private set; } = null;

		protected override void Build(OnBuilt onBuilt_)
		{
			if (SceneManager.GetActiveScene().name != "Startup")
			{
				callbackScene = SceneManager.GetActiveScene().name;
				SceneManager.LoadScene("Startup");
			}

			App.Server.Request(
			HTTPMethods.Get,
			"user",
			(JSONNode json_) =>
			{
				SceneManager.LoadScene(callbackScene);
				Json = json_;

				Email = Json["email"];
				Username = Json["username"];
				Account = Json["account"];

				onBuilt_();
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

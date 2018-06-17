using SimpleJSON;
using BestHTTP;
using System.Collections.Generic;

namespace West.Model.CloudContent
{
	public class GameSettings : Base
	{
		public JSONNode Json { get; private set; } = null;

		public int NumAbilities { get; private set; }
		public int NumClasses { get; private set; }
		public int NumKits { get; private set; }
		public int LengthConstellation { get; private set; }
        public JSONNode UpDownUpgradeSkillDictionnary { get; private set; }

        protected override void Build(OnBuilt onBuilt_)
		{
			App.Server.Request(
			HTTPMethods.Get,
			"gameSettings/Classic",
			(JSONNode json_) =>
			{
				Json = json_;

				NumAbilities = Json["numberOfAbilities"];
				NumClasses = Json["numberOfClasses"];
				NumKits = Json["numberOfKits"];
				LengthConstellation = Json["presetLength"];
                UpDownUpgradeSkillDictionnary = Json["upDownUpgradeSkillDictionnary"];

				onBuilt_();
			}).Send();
		}

		public GameSettings(Session session_)
		{
			dependencyList.Add(session_);
		}
	}
}

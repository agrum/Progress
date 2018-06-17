using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;
using UnityEngine;

namespace West.Model.CloudContent
{
	public class Account : Base
	{
		public JSONNode Json { get; private set; } = null;
		public List<ConstellationPreset> PresetList { get; private set; } = new List<ConstellationPreset>();

        public delegate void PresetDelegate(ConstellationPreset preset_);
		public event PresetDelegate PresetAdded = delegate { };
		public event PresetDelegate PresetSaved = delegate { };
		public event PresetDelegate PresetRemoved = delegate { };


		protected override void Build(OnBuilt onBuilt_)
		{
			App.Server.Request(
			HTTPMethods.Get,
			"account/" + App.Content.Session.Account,
			(JSONNode json_) =>
			{
				Json = json_;
				PresetList.Clear();
				foreach (var almostJson in Json["presets"].AsArray)
					if (almostJson.Value["constellation"] == App.Content.GameSettings.Json["constellation"])
						PresetList.Add(new ConstellationPreset(almostJson.Value));

				Debug.Log(Json);
				onBuilt_();
			}).Send();
		}

		public Account(GameSettings gameSettings_, ConstellationList constellationList_)
		{
			dependencyList.Add(gameSettings_);
			dependencyList.Add(constellationList_);
		}

		public void AddPreset()
		{
			JSONNode presetJson = new JSONObject();
			presetJson["name"] = "Preset " + PresetList.Count;
			presetJson["constellation"] = App.Content.GameSettings.Json["constellation"];
			presetJson["abilities"] = new JSONArray();
			presetJson["classes"] = new JSONArray();
			presetJson["kits"] = new JSONArray();

			var request = App.Server.Request(
				HTTPMethods.Post,
				"account/preset",
				(JSONNode json_) =>
				{
					ConstellationPreset preset = new ConstellationPreset(json_);
                    PresetList.Add(preset);
                    Json["presets"].AsArray.Add(json_);

					PresetAdded(preset);
				});
			request.AddField("preset", presetJson.ToString());
			request.Send();
        }

        public void SavePreset(ConstellationPreset preset_)
        {
            if (!PresetList.Contains(preset_))
                return;

            JSONNode presetJson = preset_.ToJson();

            var request = App.Server.Request(
                HTTPMethods.Put,
                "account/preset",
                (JSONNode json_) =>
                {
					PresetSaved(preset_);
                });
            request.AddField("preset", presetJson.ToString());
            request.Send();
        }

        public void RemovePreset(ConstellationPreset preset_)
        {
            if (!PresetList.Contains(preset_))
                return;

            App.Server.Request(
                HTTPMethods.Delete,
                "account/preset/" + preset_.Id,
                (JSONNode json_) =>
                {
                    PresetList.Remove(preset_);
                    foreach (var almostJson in Json["presets"].AsArray)
                    {
                        if (almostJson.Value["_id"] == preset_.Id)
                        {
                            Json["presets"].AsArray.Remove(almostJson.Value);
                            break;
                        }
                    }

					PresetRemoved(preset_);
                }).Send();
        }
    }
}

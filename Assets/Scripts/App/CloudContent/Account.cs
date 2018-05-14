using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using BestHTTP;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace West
{
	namespace Model
	{
		namespace CloudContent
		{
			public class Account : Base
			{
				public JSONNode Json { get; private set; } = null;
				public List<ConstellationPreset> PresetList { get; private set; } = new List<ConstellationPreset>();
				public ConstellationPreset EditedPreset { get; private set; } = null;

                public delegate void PresetAddedDelegate(ConstellationPreset preset_);
                public delegate void PresetSavedDelegate();
                public delegate void PresetRemovedDelegate();

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

				public void AddPreset(PresetAddedDelegate delegate_)
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

                            delegate_(preset);
						});
					request.AddField("preset", presetJson.ToString());
					request.Send();
                }

                public void EditPreset(ConstellationPreset preset_)
                {
                    if (!PresetList.Contains(preset_))
                        return;

                    EditedPreset = preset_;
                    SceneManager.LoadScene("PresetEditor");
                }

                public void SavePreset(ConstellationPreset preset_)
                {
                    if (!PresetList.Contains(preset_) || preset_ != EditedPreset)
                        return;

                    JSONNode presetJson = preset_.ToJson();

                    var request = App.Server.Request(
                        HTTPMethods.Put,
                        "account/preset",
                        (JSONNode json_) =>
                        {
                            EditedPreset = null;
                            SceneManager.LoadScene("PresetSelection");
                        });
                    request.AddField("preset", presetJson.ToString());
                    request.Send();
                }

                public void RemovePreset(ConstellationPreset preset_, PresetRemovedDelegate delegate_)
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

                            delegate_();
                        }).Send();
                }
            }
		}
	}
}

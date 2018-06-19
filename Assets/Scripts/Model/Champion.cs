using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;
using UnityEngine;

namespace West
{
    namespace Model
    {
        public class Champion : CloudContent.Base
        {
            public JSONNode Json { get; private set; } = null;
            public ConstellationPreset ClassPreset { get; private set; } = null;
            public List<ConstellationPreset> PresetList { get; private set; } = new List<ConstellationPreset>();

            public delegate void PresetDelegate(ConstellationPreset preset_);
            public event PresetDelegate PresetAdded = delegate { };
            public event PresetDelegate PresetSaved = delegate { };
            public event PresetDelegate PresetRemoved = delegate { };

            protected override void Build(OnBuilt onBuilt_)
            {
                App.Server.Request(
                HTTPMethods.Get,
                "champion/" + Json["_d"],
                (JSONNode json_) =>
                {
                    Json = json_;
                    PresetList.Clear();
                    foreach (var almostJson in Json["presets"].AsArray)
                        if (almostJson.Value["constellation"] == App.Content.GameSettings.Json["constellation"])
                            PresetList.Add(new ConstellationPreset(
                                almostJson.Value,
                                new PresetLimits(App.Content.GameSettings.NumAbilities, App.Content.GameSettings.NumClasses, App.Content.GameSettings.NumKits)));

                    JSONObject fakeClassPreset = new JSONObject();
                    fakeClassPreset["classes"] = Json["classes"].AsArray;
                    new ConstellationPreset(
                        fakeClassPreset,
                        new PresetLimits(0, 3, 0));

                    PresetAdded = delegate { };
                    PresetSaved = delegate { };
                    PresetRemoved = delegate { };

                    Debug.Log(Json);
                    onBuilt_();
                }).Send();
            }

            public Champion(JSONNode json_)
            {
                Json = json_;
                dependencyList.Add(App.Content.GameSettings);
                dependencyList.Add(App.Content.SkillList);
            }

            public void Detach()
            {
                PresetAdded = null;
                PresetSaved = null;
                PresetRemoved = null;
                loaded = false;
            }

            public void AddEmptyPreset()
            {
                if (!loaded)
                    return;

                JSONNode presetJson = new JSONObject();
                presetJson["name"] = "Preset " + PresetList.Count;
                presetJson["constellation"] = App.Content.GameSettings.Json["constellation"];
                presetJson["abilities"] = new JSONArray();
                presetJson["classes"] = new JSONArray();
                presetJson["kits"] = new JSONArray();

                var request = App.Server.Request(
                    HTTPMethods.Post,
                    "preset",
                    (JSONNode json_) =>
                    {
                        ConstellationPreset preset = new ConstellationPreset(
                            json_,
                            new PresetLimits(App.Content.GameSettings.NumAbilities, App.Content.GameSettings.NumClasses, App.Content.GameSettings.NumKits));
                        PresetList.Add(preset);
                        Json["presets"].AsArray.Add(json_);

                        PresetAdded(preset);
                    });
                request.AddField("preset", presetJson.ToString());
                request.Send();
            }

            public void SavePreset(ConstellationPreset preset_)
            {
                if (!loaded)
                    return;

                if (!PresetList.Contains(preset_))
                    return;

                JSONNode presetJson = preset_.ToJson();

                var request = App.Server.Request(
                    HTTPMethods.Put,
                    "preset",
                    (JSONNode json_) =>
                    {
                        PresetSaved(preset_);
                    });
                request.AddField("preset", presetJson.ToString());
                request.Send();
            }

            public void RemovePreset(ConstellationPreset preset_)
            {
                if (!loaded)
                    return;

                if (!PresetList.Contains(preset_))
                    return;

                App.Server.Request(
                    HTTPMethods.Delete,
                    "preset/" + preset_.Id,
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
}

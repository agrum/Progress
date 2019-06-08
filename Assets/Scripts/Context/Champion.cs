using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Champion : CloudContent.Base
    {
        public JSONObject Json { get; private set; } = null;
        public ConstellationPreset ClassPreset { get; private set; } = null;
        public List<ConstellationPreset> PresetList { get; private set; } = new List<ConstellationPreset>();
        public ChampionUpgrades Upgrades { get; private set; } = null;
        public string Name { get { return Json["name"]; } set { Json["name"] = value; } }
        public string Level { get { return Json["level"]; } set { Json["level"] = value; } }
        public int SpecializationPoints { get { return Json["specializationPoints"]; } set { Json["specializationPoints"] = value; } }

        public delegate void PresetDelegate(ConstellationPreset preset_);
        public event PresetDelegate PresetAdded = delegate { };
        public event PresetDelegate PresetSaved = delegate { };
        public event PresetDelegate PresetRemoved = delegate { };

        protected override void Build(OnBuilt onBuilt_)
        {
            App.Server.Request(
            HTTPMethods.Get,
            "champion/" + Json["_id"],
            (JSONNode json_) =>
            {
                Json = json_.AsObject;
                PresetList.Clear();
                foreach (var almostJson in Json["presets"].AsArray)
                    if (almostJson.Value["constellation"] == App.Content.GameSettings.Json["constellation"])
                        PresetList.Add(new ConstellationPreset(
                            almostJson.Value.AsObject,
                            new PresetLimits(App.Content.GameSettings.NumAbilities, App.Content.GameSettings.NumClasses, App.Content.GameSettings.NumKits)));

                PopulateFakeClassPreset(Json["classes"].AsArray);

                Upgrades = new ChampionUpgrades(Json["skillUpgrades"].AsArray);

                PresetAdded = delegate { };
                PresetSaved = delegate { };
                PresetRemoved = delegate { };

                Debug.Log(Json);
                onBuilt_();
            }).Send();
        }

        public Champion(JSONObject json_)
        {
            Json = json_;
            PopulateFakeClassPreset(Json["classes"].AsArray);
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
                        json_.AsObject,
                        new PresetLimits(App.Content.GameSettings.NumAbilities, App.Content.GameSettings.NumClasses, App.Content.GameSettings.NumKits));
                    PresetList.Add(preset);
                    Json["presets"].AsArray.Add(json_);

                    PresetAdded(preset);
                });
            request.AddHeader("Content-Type", "application/json");
            request.RawData = System.Text.Encoding.UTF8.GetBytes(presetJson.ToString());
            request.Send();
        }

        public void SavePreset(ConstellationPreset preset_)
        {
            if (!loaded)
                return;

            if (preset_.Id != null && !PresetList.Contains(preset_))
                return;

            JSONObject presetJson = preset_.ToJson();
            HTTPRequest request;
            if (preset_.Id == null)
                request = App.Server.Request(
                    HTTPMethods.Post,
                    "champion/" + Json["_id"] + "/preset",
                    (JSONNode json_) =>
                    {
                        ConstellationPreset preset = new ConstellationPreset(
                            json_.AsObject,
                            new PresetLimits(App.Content.GameSettings.NumAbilities, App.Content.GameSettings.NumClasses, App.Content.GameSettings.NumKits));
                        PresetList.Add(preset);
                        Json["presets"].AsArray.Add(json_);

                        PresetSaved(preset_);
                    });
            else
                request = App.Server.Request(
                    HTTPMethods.Put,
                    "champion/" + Json["_id"] + "/preset/",
                    (JSONNode json_) =>
                    {
                        PresetSaved(preset_);
                    });
            request.AddHeader("Content-Type", "application/json");
            request.RawData = System.Text.Encoding.UTF8.GetBytes(presetJson.ToString());
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
               "champion/" + Json["_id"] + "/preset/" + preset_.Id,
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

        private void PopulateFakeClassPreset(JSONArray array_)
        {
            JSONObject fakeClassPreset = new JSONObject();
            fakeClassPreset["classes"] = array_;
            ClassPreset = new ConstellationPreset(
                fakeClassPreset,
                new PresetLimits(0, 3, 0));
        }
    }
}

using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;
using UnityEngine;

namespace West.CloudContent
{
    public class Account : Base
    {
        public JSONNode Json { get; private set; } = null;
        public List<Model.Champion> ChampionList { get; private set; } = new List<Model.Champion>();
        public Model.Champion ActiveChampion { get; private set; } = null;

        public delegate void VoidDelegate();
        public delegate void ChampionDelegate(Model.Champion champion_);
        public event VoidDelegate ActiveChampionChanged = delegate { };
        public event ChampionDelegate ChampionAdded = delegate { };
        public event ChampionDelegate ChampionSaved = delegate { };
        public event ChampionDelegate ChampionRemoved = delegate { };

        protected override void Build(OnBuilt onBuilt_)
        {
            App.Server.Request(
            HTTPMethods.Get,
            "account/" + App.Content.Session.Account,
            (JSONNode json_) =>
            {
                Json = json_;
                ChampionList.Clear();
                foreach (var almostJson in Json["champions"].AsArray)
                    ChampionList.Add(new Model.Champion(almostJson.Value.AsObject));

                Debug.Log(Json);
                onBuilt_();
            }).Send();
        }

        public Account(SkillList skillList_)
        {
            dependencyList.Add(skillList_);
        }

        public void ActivateChampion(Model.Champion champion_)
        {
            if (!loaded)
                return;

            if (champion_ == null || !ChampionList.Contains(champion_))
                return;

            if (ActiveChampion != null)
                ActiveChampion.Detach();

            ActiveChampion = champion_;
            ActiveChampionChanged();
        }

        public void AddChampion(string name, JSONArray classes)
        {
            if (!loaded)
                return;

            JSONObject championJson = new JSONObject();
            championJson["name"] = name;
            championJson["classes"] = classes;

            var request = App.Server.Request(
                HTTPMethods.Post,
                "champion",
                (JSONNode json_) =>
                {
                    Model.Champion champion = new Model.Champion(json_.AsObject);
                    ChampionList.Add(champion);
                    Json["champions"].AsArray.Add(json_);

                    ChampionAdded(champion);
                });
            request.AddField("champion", championJson);
            request.Send();
        }

        public void RemoveChampion(Model.Champion champion_)
        {
            if (!loaded)
                return;

            if (!ChampionList.Contains(champion_))
                return;

            App.Server.Request(
                HTTPMethods.Delete,
                "champion/" + champion_.Json["_id"],
                (JSONNode json_) =>
                {
                    ChampionList.Remove(champion_);
                    foreach (var almostJson in Json["champions"].AsArray)
                    {
                        if (almostJson.Value["_id"] == champion_.Json["_id"])
                        {
                            Json["champions"].AsArray.Remove(almostJson.Value);
                            break;
                        }
                    }

                    ChampionRemoved(champion_);
                }).Send();
        }
    }
}
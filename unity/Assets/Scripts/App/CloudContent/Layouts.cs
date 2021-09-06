using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;
using System.Collections;

namespace Assets.Scripts.CloudContent
{
    public class Layouts : Base
    {
        public JSONNode Json { get; private set; } = null;

        public Dictionary<string, Data.Layout.Outdoor> OutdoorLayouts = new Dictionary<string, Data.Layout.Outdoor>();

        protected override IEnumerator Build()
        {
            yield return App.Server.Request(
            HTTPMethods.Get,
            "layout/outdoor",
            (JSONNode json_) =>
            {
                Json = json_;

                foreach (var almostJson in Json)
                {
                    OutdoorLayouts.Add(almostJson.Value["_id"], new Data.Layout.Outdoor(almostJson.Value));
                }
            }).Send();
        }

        public Layouts(Session session_)
        {
            dependencyList.Add(session_);
        }
    }
}

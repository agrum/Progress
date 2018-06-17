using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;

namespace West.Model.CloudContent
{
    public class SkillList : Base
    {
        public JSONNode Json { get; private set; } = null;

        public Ability Ability(string uuid_)
        {
            return abilityTable[uuid_] as Ability;
        }

        public Class Class(string uuid_)
        {
            return classTable[uuid_] as Class;
        }

        public Kit Kit(string uuid_)
        {
            return kitTable[uuid_] as Kit;
        }

        private Dictionary<string, Skill> abilityTable = new Dictionary<string, Skill>();
        private Dictionary<string, Skill> classTable = new Dictionary<string, Skill>();
        private Dictionary<string, Skill> kitTable = new Dictionary<string, Skill>();

        protected override void Build(OnBuilt onBuilt_)
        {
            App.Server.Request(
            HTTPMethods.Get,
            "skill",
            (JSONNode json_) =>
            {
                Json = json_;

                foreach (var almostJson in Json)
                {
                    if (almostJson.Value["type"] == "ability")
                        abilityTable.Add(almostJson.Value["_id"], new Ability(almostJson.Value));
                    else if (almostJson.Value["type"] == "class")
                        classTable.Add(almostJson.Value["_id"], new Class(almostJson.Value));
                    else if (almostJson.Value["type"] == "kit")
                        kitTable.Add(almostJson.Value["_id"], new Kit(almostJson.Value));
                }

                onBuilt_();
            }).Send();
        }

        public SkillList(GameSettings gameSettings_)
        {
            dependencyList.Add(gameSettings_);
        }
    }
}

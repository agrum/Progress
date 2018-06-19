using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;

namespace West.CloudContent
{
    public class SkillList : Base
    {
        public JSONNode Json { get; private set; } = null;

        public Model.Ability Ability(string uuid_)
        {
            return abilityTable[uuid_] as Model.Ability;
        }

        public Model.Class Class(string uuid_)
        {
            return classTable[uuid_] as Model.Class;
        }

        public Model.Kit Kit(string uuid_)
        {
            return kitTable[uuid_] as Model.Kit;
        }

        private Dictionary<string, Model.Skill> abilityTable = new Dictionary<string, Model.Skill>();
        private Dictionary<string, Model.Skill> classTable = new Dictionary<string, Model.Skill>();
        private Dictionary<string, Model.Skill> kitTable = new Dictionary<string, Model.Skill>();

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
                        abilityTable.Add(almostJson.Value["_id"], new Model.Ability(almostJson.Value));
                    else if (almostJson.Value["type"] == "class")
                        classTable.Add(almostJson.Value["_id"], new Model.Class(almostJson.Value));
                    else if (almostJson.Value["type"] == "kit")
                        kitTable.Add(almostJson.Value["_id"], new Model.Kit(almostJson.Value));
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

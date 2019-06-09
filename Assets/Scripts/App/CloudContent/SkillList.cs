using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;

namespace Assets.Scripts.CloudContent
{
    public class SkillList : Base
    {
        public JSONNode Json { get; private set; } = null;

        public Dictionary<string, Data.Skill.Skill> Abilities = new Dictionary<string, Data.Skill.Skill>();
        public Dictionary<string, Data.Skill.Skill> Classes = new Dictionary<string, Data.Skill.Skill>();
        public Dictionary<string, Data.Skill.Skill> Kits = new Dictionary<string, Data.Skill.Skill>();

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
                    if (almostJson.Value["Type"] == "Ability")
                        Abilities.Add(almostJson.Value["_id"], new Data.Skill.Skill(almostJson.Value));
                    else if (almostJson.Value["Type"] == "Class")
                        Classes.Add(almostJson.Value["_id"], new Data.Skill.Skill(almostJson.Value));
                    else if (almostJson.Value["Type"] == "Kit")
                        Kits.Add(almostJson.Value["_id"], new Data.Skill.Skill(almostJson.Value));
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

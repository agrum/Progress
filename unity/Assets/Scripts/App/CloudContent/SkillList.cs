using System.Collections.Generic;
using SimpleJSON;
using BestHTTP;
using System.Collections;

namespace Assets.Scripts.CloudContent
{
    public class SkillList : Base
    {
        public JSONNode Json { get; private set; } = null;

        public Dictionary<string, Data.Skill.Skill> Abilities = new Dictionary<string, Data.Skill.Skill>();
        public Dictionary<string, Data.Skill.Skill> Classes = new Dictionary<string, Data.Skill.Skill>();
        public Dictionary<string, Data.Skill.Skill> Kits = new Dictionary<string, Data.Skill.Skill>();

        protected override IEnumerator Build()
        {
            yield return App.Server.Request(
            HTTPMethods.Get,
            "skill",
            (JSONNode json_) =>
            {
                Json = json_;

                foreach (var almostJson in Json)
                {
                    if (almostJson.Value["category"] == "Ability")
                        Abilities.Add(almostJson.Value["_id"], new Data.Skill.Skill(almostJson.Value));
                    else if (almostJson.Value["category"] == "Class")
                        Classes.Add(almostJson.Value["_id"], new Data.Skill.Skill(almostJson.Value));
                    else if (almostJson.Value["category"] == "Kit")
                        Kits.Add(almostJson.Value["_id"], new Data.Skill.Skill(almostJson.Value));
                }
            }).Send();
        }

        public SkillList(GameSettings gameSettings_)
        {
            dependencyList.Add(gameSettings_);
        }
    }
}

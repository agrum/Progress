using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    public class ChampionHeadline : WestBehaviour
    {
        public WestText championName = null;
        public WestText level = null;
        public WestText gear = null;

        public void Setup()
        {
            Debug.Assert(championName != null);
            Debug.Assert(level != null);
            Debug.Assert(gear != null);

            App.Content.Account.ActiveChampion?.Load(() =>
            {
                Delay(() =>
                {
                    championName.text = App.Content.Account.ActiveChampion.Json["name"];
                    level.text = level.Reference.Replace("#0#", App.Content.Account.ActiveChampion.Json["level"]);
                    gear.text = level.Reference.Replace("#0#", App.Content.Account.ActiveChampion.Json["gear"]);
                });
            });
        }
    }
}

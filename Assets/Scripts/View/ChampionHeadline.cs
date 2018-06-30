using UnityEngine;
using UnityEngine.UI;

namespace West.View
{
    public class ChampionHeadline : WestBehaviour
    {
        public Text championName = null;
        public Text level = null;
        public Text gear = null;

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
                    level.text = level.text.Replace("#0#", App.Content.Account.ActiveChampion.Json["level"]);
                    gear.text = level.text.Replace("#0#", App.Content.Account.ActiveChampion.Json["gear"]);
                });
            });
        }
    }
}

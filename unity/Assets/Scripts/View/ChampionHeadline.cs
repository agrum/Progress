using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
    public class ChampionHeadline : MonoBehaviour
    {
        public WestText championName = null;
        public WestText level = null;
        public WestText gear = null;

        public IEnumerator Start()
        {
            Debug.Assert(championName != null);
            Debug.Assert(level != null);
            Debug.Assert(gear != null);

            yield return StartCoroutine(App.Content.Account.ActiveChampion?.Load());

            championName.text = App.Content.Account.ActiveChampion.Json["name"];
            level.Format(App.Content.Account.ActiveChampion.Json["level"]);
            gear.Format(App.Content.Account.ActiveChampion.Json["gear"]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class NodeTextualDetails : MonoBehaviour
	{
		public WestText Name = null;
		public WestText Ability = null;
		public WestText Class = null;
		public WestText Kit = null;
		public WestText Description = null;
		public WestText Details = null;
		public WestText Cooldown = null;
		public WestText CastTime = null;
		public WestText Modifier = null;
		public WestText Unit = null;
		public WestText Projectile = null;
        public WestText Charge = null;
        public WestText Stack = null;
        public WestText KitDetails = null;

        ViewModel.NodeTextualDetails viewModel;

        private string colorPrefix;
        private string colorSuffix;

        public void SetContext(ViewModel.NodeTextualDetails viewModel_)
        {
            Debug.Assert(viewModel_ != null);

            viewModel = viewModel_;
            viewModel.SkillChanged += OnSkillChanged;
        }

        void OnDestroy()
        {
            if (viewModel == null)
                return;

            viewModel.SkillChanged -= OnSkillChanged;
            viewModel = null;
        }

		private void OnSkillChanged(JSONNode skill)
		{
			bool active = (skill != null);

			Name.gameObject.SetActive(active);
			Description.gameObject.SetActive(active);
			Details.gameObject.SetActive(active);
			Cooldown.gameObject.SetActive(active);
			CastTime.gameObject.SetActive(active);
			Modifier.gameObject.SetActive(active);
			Unit.gameObject.SetActive(active);
			Projectile.gameObject.SetActive(active);
			Charge.gameObject.SetActive(active);
			KitDetails.gameObject.SetActive(active);

			Ability.gameObject.SetActive(false);
			Class.gameObject.SetActive(false);
			Kit.gameObject.SetActive(false);

			if (!active)
				return;
				
			colorPrefix = "<color=#" + skill["color"] + ">";
			colorSuffix = "</color>";

			Name.FormatPair("#content#", skill["name"]);
			Description.FormatPair("#content#", skill["description"]);

            if (skill["typeName"] == "Abilities")
            {
                Ability.text = colorPrefix + Ability.Reference + colorSuffix;
                Ability.gameObject.SetActive(true);
            }
            else if (skill["typeName"] == "Classes")
            {
                Class.text = colorPrefix + Class.Reference + colorSuffix;
                Class.gameObject.SetActive(true);
            }
            else if (skill["typeName"] == "Kits")
            { 
				Kit.text = colorPrefix + Kit.Reference + colorSuffix;
                Kit.gameObject.SetActive(true);
			}
                
            Cooldown.gameObject.SetActive(false);
            CastTime.gameObject.SetActive(false);
            Modifier.gameObject.SetActive(false);
            Projectile.gameObject.SetActive(false);
            Unit.gameObject.SetActive(false);
            Charge.gameObject.SetActive(false);
            KitDetails.gameObject.SetActive(false);

            WestString description = skill["details"];
            Details.Format("#content#", description.Format(viewModel.Desc));

            if (viewModel.Misc.ContainsKey("cooldown"))
            {
                Cooldown.gameObject.SetActive(true);
                Cooldown.FormatPair("cooldown", viewModel.Misc["cooldown"]);
            }
            else if (viewModel.Misc.ContainsKey("castTime"))
            {
                CastTime.gameObject.SetActive(true);
                CastTime.FormatPair("castTime", viewModel.Misc["castTime"]);
            }

            if (viewModel.Modifier.Count > 0)
            {
                Modifier.gameObject.SetActive(true);
                string[] lineSplit = Modifier.Reference.Split('\n');
                Modifier.text = lineSplit[0];
                Modifier.text += Append(lineSplit[1], "range", viewModel.Modifier);
                Modifier.text += Append(lineSplit[2], "duration", viewModel.Modifier);
                Modifier.text += Append(lineSplit[3], "stack", viewModel.Modifier);
            }

            if (viewModel.Unit.Count > 0)
            {
                Unit.gameObject.SetActive(true);
                string[] lineSplit = Unit.Reference.Split('\n');
                Unit.text = lineSplit[0];
                Unit.text += Append(lineSplit[1], "hp", viewModel.Unit);
                Unit.text += Append(lineSplit[2], "width", viewModel.Unit);
                Unit.text += Append(lineSplit[3], "duration", viewModel.Unit);
                Unit.text += Append(lineSplit[4], "vision", viewModel.Unit);
                Unit.text += Append(lineSplit[5], "placementRange", viewModel.Unit);
            }

            if (viewModel.Kit.Count > 0)
            {
                Kit.gameObject.SetActive(true);
                string[] lineSplit = Kit.Reference.Split('\n');
                Kit.text = lineSplit[0];
                Kit.text += Append(lineSplit[1], "hp", viewModel.Kit);
                Kit.text += Append(lineSplit[2], "armor", viewModel.Kit);
                Kit.text += Append(lineSplit[3], "shield", viewModel.Kit);
                Kit.text += Append(lineSplit[4], "damage", viewModel.Kit);
                Kit.text += Append(lineSplit[5], "rate", viewModel.Kit);
                Kit.text += Append(lineSplit[6], "range", viewModel.Kit);
                Kit.text += Append(lineSplit[7], "angle", viewModel.Kit);
                Kit.text += Append(lineSplit[8], "speed", viewModel.Kit);
            }

            if (viewModel.Projectile.Count > 0)
            {
                Projectile.gameObject.SetActive(true);
                Projectile.Format(viewModel.Projectile);
            }

            if (viewModel.Charge.Count > 0)
            {
                Charge.gameObject.SetActive(true);
                Charge.Format(viewModel.Charge);
            }

            if (viewModel.Stack.Count > 0)
            {
                Stack.gameObject.SetActive(true);
                Stack.Format(viewModel.Stack);
            }
        }

        private string Append(string text_, string pattern_, Dictionary<string, string> collection_)
        {
            if (collection_.ContainsKey(pattern_))
                return "\n" + new WestString(text_).FormatPair(pattern_, collection_[pattern_]);
            return null;
        }
	}
}
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
		public Text Name = null;
		public Text Ability = null;
		public Text Class = null;
		public Text Kit = null;
		public Text Description = null;
		public Text Details = null;
		public Text Cooldown = null;
		public Text CastTime = null;
		public Text Modifier = null;
		public Text Unit = null;
		public Text Projectile = null;
		public Text Charge = null;
		public Text KitDetails = null;

        ViewModel.NodeTextualDetails viewModel;

        private string nameString;
		private string abilityString;
		private string classString;
		private string kitString;
		private string descriptionString;
		private string detailsString;
		private string cooldownString;
		private string castTimeString;
		private string modifierString;
		private string unitString;
		private string projectileString;
		private string chargeString;
		private string kitDetailsString;

        private string colorPrefix;
        private string colorSuffix;

        void Start()
		{
			nameString = Name.text;
			abilityString = Ability.text;
			classString = Class.text;
			kitString = Kit.text;
			descriptionString = Description.text;
			detailsString = Details.text; ;
			cooldownString = Cooldown.text;
			castTimeString = CastTime.text;
			modifierString = Modifier.text;
			unitString = Unit.text;
			projectileString = Projectile.text;
			chargeString = Charge.text;
			kitDetailsString = KitDetails.text;
		}

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

			Name.text = nameString.Replace("#content#", skill["name"]);
			Description.text = descriptionString.Replace("#content#", skill["description"]);

            if (skill["typeName"] == "Abilities")
            {
                Ability.text = colorPrefix + abilityString + colorSuffix;
                Ability.gameObject.SetActive(true);
            }
            else if (skill["typeName"] == "Classes")
            {
                Class.text = colorPrefix + classString + colorSuffix;
                Class.gameObject.SetActive(true);
            }
            else if (skill["typeName"] == "Kits")
            { 
				Kit.text = colorPrefix + kitString + colorSuffix;
                Kit.gameObject.SetActive(true);
			}
                
            Cooldown.gameObject.SetActive(false);
            CastTime.gameObject.SetActive(false);
            Modifier.gameObject.SetActive(false);
            Projectile.gameObject.SetActive(false);
            Unit.gameObject.SetActive(false);
            Charge.gameObject.SetActive(false);
            KitDetails.gameObject.SetActive(false);

            Details.text = detailsString.Replace("#content#", skill["details"]);
            if (viewModel.Desc != null)
            {
                foreach (var entry in viewModel.Desc)
                {
                    Details.text = Replace(Details.text, entry.Key, entry.Value);
                }
            }

            if (viewModel.Misc != null)
            {
                if (viewModel.Misc.ContainsKey("cooldown"))
                {
                    Cooldown.gameObject.SetActive(true);
                    Cooldown.text = Replace(cooldownString, "cooldown", viewModel.Misc["cooldown"]);
                }
                else if (viewModel.Misc.ContainsKey("castTime"))
                {
                    CastTime.gameObject.SetActive(true);
                    CastTime.text = Replace(castTimeString, "castTime", viewModel.Misc["castTime"]);
                }
            }
                
            if (viewModel.Modifler != null)
            {
                Modifier.gameObject.SetActive(true);
                string[] lineSplit = modifierString.Split('\n');
                Modifier.text = lineSplit[0];
                if (viewModel.Modifler.ContainsKey("range"))
                    Modifier.text += "\n" + Replace(lineSplit[1], "range", viewModel.Modifler["range"]);
                if (viewModel.Modifler.ContainsKey("duration"))
                    Modifier.text += "\n" + Replace(lineSplit[2], "duration", viewModel.Modifler["duration"]);
                if (viewModel.Modifler.ContainsKey("stack"))
                    Modifier.text += "\n" + Replace(lineSplit[3], "stack", viewModel.Modifler["stack"]);
            }

			/*if (metrics["projectile"].IsObject)
			{
				JSONNode projectiletNode = metrics["projectile"];
				Projectile.text = projectileString.Replace("#range#", colorPrefix + projectiletNode["range"] + colorSuffix);
				Projectile.text = Projectile.text.Replace("#speed#", colorPrefix + projectiletNode["speed"] + colorSuffix);
				Projectile.text = Projectile.text.Replace("#width#", colorPrefix + projectiletNode["width"] + colorSuffix);
			}
			else
				Projectile.gameObject.SetActive(false);

			if (metrics["unit"].IsObject)
			{
				JSONNode unitNode = metrics["unit"];
				string[] lineSplit = unitString.Split('\n');
				Unit.text = lineSplit[0];
				if (unitNode["hp"].AsDouble != 0)
					Unit.text += "\n" + lineSplit[1].Replace("#hp#", colorPrefix + unitNode["hp"] + colorSuffix);
				if (unitNode["width"].AsDouble != 0)
					Unit.text += "\n" + lineSplit[2].Replace("#width#", colorPrefix + unitNode["width"] + colorSuffix);
				if (unitNode["duration"].AsDouble != 0)
					Unit.text += "\n" + lineSplit[3].Replace("#duration#", colorPrefix + unitNode["duration"] + colorSuffix);
				if (unitNode["vision"].AsDouble != 0)
					Unit.text += "\n" + lineSplit[4].Replace("#vision#", colorPrefix + unitNode["vision"] + colorSuffix);
				if (unitNode["range"].AsDouble != 0)
					Unit.text += "\n" + lineSplit[5].Replace("#placementRange#", colorPrefix + unitNode["placementRange"] + colorSuffix);
			}
			else
				Unit.gameObject.SetActive(false);

			if (metrics["charge"].IsObject)
			{
				JSONNode chargekNode = metrics["charge"];
				Charge.text = chargeString.Replace("#max#", colorPrefix + chargekNode["max"] + colorSuffix);
				Charge.text = Charge.text.Replace("#duration#", colorPrefix + chargekNode["duration"] + colorSuffix);
			}
			else
				Charge.gameObject.SetActive(false);

			if (metrics["kit"].IsObject)
			{
				JSONNode kitNode = metrics["kit"];
				string[] lineSplit = kitDetailsString.Split('\n');
				KitDetails.text = lineSplit[0];
				if (kitNode["life"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[1].Replace("#hp#", colorPrefix + kitNode["life"] + colorSuffix);
				if (kitNode["armor"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[2].Replace("#armor#", colorPrefix + kitNode["armor"] + colorSuffix);
				if (kitNode["shield"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[3].Replace("#shield#", colorPrefix + kitNode["shield"] + colorSuffix);
				if (kitNode["damage"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[4].Replace("#damage#", colorPrefix + kitNode["damage"] + colorSuffix);
				if (kitNode["rate"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[5].Replace("#rate#", colorPrefix + kitNode["rate"] + colorSuffix);
				if (kitNode["range"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[6].Replace("#range#", colorPrefix + kitNode["range"] + colorSuffix);
				if (kitNode["angle"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[7].Replace("#angle#", colorPrefix + kitNode["angle"] + colorSuffix);
				if (kitNode["speed"].AsDouble != 0)
					KitDetails.text += "\n" + lineSplit[8].Replace("#speed#", colorPrefix + kitNode["speed"] + colorSuffix);
			}
			else
				KitDetails.gameObject.SetActive(false);*/
		}

        private string Replace(string text, string pattern, string replacement)
        {
            if (text.Contains("#" + pattern + "#"))
                return text.Replace("#" + pattern + "#", colorPrefix + replacement + colorSuffix);

            return text.Replace("%" + pattern + "%", colorPrefix + (Convert.ToDouble(replacement) * 100) + "%" + colorSuffix);
        }
	}
}
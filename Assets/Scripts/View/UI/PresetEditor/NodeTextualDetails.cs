using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace West
{
	namespace View
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
			public GameObject ExitPanel = null;

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
			}

			public void Setup(ConstellationNode node)
			{
				bool active = (node != null && node.Model != null);

				Name.gameObject.SetActive(active);
				Description.gameObject.SetActive(active);
				Details.gameObject.SetActive(active);
				Cooldown.gameObject.SetActive(active);
				CastTime.gameObject.SetActive(active);
				Modifier.gameObject.SetActive(active);
				Unit.gameObject.SetActive(active);
				Projectile.gameObject.SetActive(active);
				Charge.gameObject.SetActive(active);
				ExitPanel.SetActive(!active);

				Ability.gameObject.SetActive(false);
				Class.gameObject.SetActive(false);
				Kit.gameObject.SetActive(false);

				if (!active)
					return;

				var nodeModel = node.Model;
				JSONNode metrics = nodeModel.Json["metrics"];

				string colorPrefix = "<color=#" + ColorUtility.ToHtmlStringRGBA(node.Mat.color) + ">";
				string colorSuffix = "</color>";

				Name.text = nameString.Replace("#content#", nodeModel.Json["name"]);
				Description.text = descriptionString.Replace("#content#", nodeModel.Json["description"]);

				switch (node.Model.Type)
				{
					case Model.ConstellationNode.NodeType.Ability:
						Ability.text = colorPrefix + abilityString + colorSuffix;
						Ability.gameObject.SetActive(true);
						break;
					case Model.ConstellationNode.NodeType.Class:
						Class.text = colorPrefix + classString + colorSuffix;
						Class.gameObject.SetActive(true);
						break;
					case Model.ConstellationNode.NodeType.Kit:
						Kit.text = colorPrefix + kitString + colorSuffix;
						Kit.gameObject.SetActive(true);
						break;
				}

				Details.text = detailsString.Replace("#content#", nodeModel.Json["details"]);
				foreach (var metric in metrics)
				{
					if (metric.Key == "cooldown"
						|| metric.Key == "castTime"
						|| metric.Key == "modifier"
						|| metric.Key == "projectile"
						|| metric.Key == "unit"
						|| metric.Key == "stack"
						|| metric.Key == "charge")
						continue;

					Details.text = Details.text.Replace("#" + metric.Key + "#", colorPrefix + metric.Value + colorSuffix);
				}

				if (metrics["cooldown"].IsNumber)
					Cooldown.text = cooldownString.Replace("#cooldown#", colorPrefix + metrics["cooldown"] + colorSuffix);
				else
					Cooldown.gameObject.SetActive(false);

				if (metrics["castTime"].IsNumber)
					CastTime.text = castTimeString.Replace("#castTime#", colorPrefix + metrics["castTime"] + colorSuffix);
				else
					CastTime.gameObject.SetActive(false);

				if (metrics["modifier"].IsObject)
				{
					JSONNode modifiertNode = metrics["modifier"];
					string[] lineSplit = modifierString.Split('\n');
					Modifier.text = lineSplit[0];
					if (modifiertNode["range"].AsDouble != 0)
						Modifier.text += "\n" + lineSplit[1].Replace("#range#", colorPrefix + modifiertNode["range"] + colorSuffix);
					if (modifiertNode["duration"].AsDouble != 0)
						Modifier.text += "\n" + lineSplit[2].Replace("#duration#", colorPrefix + modifiertNode["duration"] + colorSuffix);
					if (modifiertNode["stack"].AsDouble != 0)
						Modifier.text += "\n" + lineSplit[3].Replace("#stack#", colorPrefix + modifiertNode["stack"] + colorSuffix);
				}
				else
					Modifier.gameObject.SetActive(false);

				if (metrics["projectile"].IsObject)
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
					if (unitNode["placementRange"].AsDouble != 0)
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
			}
		}
	}
}
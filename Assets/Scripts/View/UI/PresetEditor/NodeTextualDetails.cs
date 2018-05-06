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
			public Text Description = null;
			public Text Details = null;
			public Text Cooldown = null;
			public Text CastTime = null;
			public Text Unit = null;
			public Text Projectile = null;
			public Text Stack = null;
			public Text Charge = null;

			private string nameString;
			private string descriptionString;
			private string detailsString;
			private string cooldownString;
			private string castTimeString;
			private string unitString;
			private string projectileString;
			private string stackString;
			private string chargeString;

			void Start()
			{
				nameString = Name.text;
				descriptionString = Description.text;
				detailsString = Details.text; ;
				cooldownString = Cooldown.text;
				castTimeString = CastTime.text;
				unitString = Unit.text;
				projectileString = Projectile.text;
				stackString = Stack.text;
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
				Unit.gameObject.SetActive(active);
				Projectile.gameObject.SetActive(active);
				Stack.gameObject.SetActive(active);
				Charge.gameObject.SetActive(active);

				if (!active)
					return;

				var nodeModel = node.Model;
				JSONNode metrics = nodeModel.Json["metrics"];

				string colorPrefix = "<color=#" + ColorUtility.ToHtmlStringRGBA(node.Mat.color) + ">";
				string colorSuffix = "</color>";

				Name.text = nameString.Replace("#content#", nodeModel.Json["name"]);
				Description.text = descriptionString.Replace("#content#", nodeModel.Json["description"]);

				Details.text = detailsString.Replace("#content#", nodeModel.Json["details"]);
				foreach (var metric in metrics)
				{
					if (metric.Key == "cooldown"
						|| metric.Key == "castTime"
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
					Unit.text = unitString.Replace("#hp#", colorPrefix + unitNode["hp"] + colorSuffix);
					Unit.text = Unit.text.Replace("#vision#", colorPrefix + unitNode["vision"] + colorSuffix);
					Unit.text = Unit.text.Replace("#duration#", colorPrefix + unitNode["duration"] + colorSuffix);
					Unit.text = Unit.text.Replace("#width#", colorPrefix + unitNode["width"] + colorSuffix);
				}
				else
					Unit.gameObject.SetActive(false);

				if (metrics["stack"].IsObject)
				{
					JSONNode stackNode = metrics["stack"];
					Stack.text = stackString.Replace("#max#", colorPrefix + stackNode["max"] + colorSuffix);
					Stack.text = Stack.text.Replace("#duration#", colorPrefix + stackNode["duration"] + colorSuffix);
				}
				else
					Stack.gameObject.SetActive(false);

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
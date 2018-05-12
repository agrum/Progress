using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BestHTTP;
using SimpleJSON;
using UnityEngine;

namespace West
{
	namespace Model
	{
		public class Constellation
		{
			public JSONNode Json { get; private set; } = null;
			public List<int> StartingAbilityNodeIndexList { get; private set; } = new List<int>();
			private List<ConstellationNode> abilityNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> classNodeList = new List<ConstellationNode>();
			private List<ConstellationNode> kitNodeList = new List<ConstellationNode>();
			private Vector2 halfSize = new Vector2(0, 0);

			public List<ConstellationNode> AbilityNodeList
			{
				get { return abilityNodeList; }
			}

			public List<ConstellationNode> ClassNodeList
			{
				get { return classNodeList; }
			}

			public List<ConstellationNode> KitNodeList
			{
				get { return kitNodeList; }
			}

			public Vector2 HalfSize
			{
				get { return halfSize; }
			}

			public Constellation(JSONNode json_)
			{
				Json = json_;

				JSONArray startingAbilityIndexArray = Json["startingAbilities"].AsArray;

				//find starting node indexes
				foreach (var startingAbilityindex in startingAbilityIndexArray)
					StartingAbilityNodeIndexList.Add(startingAbilityindex.Value.AsInt);

				//find scale factor
				JSONArray abilityArray = Json["abilities"].AsArray;
				foreach (var abilityNode in abilityArray)
				{
					JSONNode ability = abilityNode.Value;
					if (Math.Abs(ability["position"]["x"].AsFloat) > HalfSize.x)
						halfSize.x = Math.Abs(ability["position"]["x"].AsFloat);
					if (Math.Abs(ability["position"]["y"].AsFloat) > HalfSize.y)
						halfSize.y = Math.Abs(ability["position"]["y"].AsFloat);
				}

				//create constellation nodes
				PopulateNodes(
					abilityArray,
					Skill.TypeEnum.Ability,
					ref abilityNodeList);
				PopulateNodes(
					Json["classes"].AsArray,
					Skill.TypeEnum.Class,
					ref classNodeList);
				PopulateNodes(
					Json["kits"].AsArray,
					Skill.TypeEnum.Kit,
					ref kitNodeList);

				//connect nodes directly
				JSONArray abilityToAbilityLinkArray = Json["abilityToAbilityLinks"].AsArray;
				foreach (var abilityToAbilityLink in abilityToAbilityLinkArray)
				{
					JSONArray link = abilityToAbilityLink.Value.AsArray;
					new ConstellationNodeLink(abilityNodeList[link[0].AsInt], abilityNodeList[link[1].AsInt]);
				}
				JSONArray classToAbilityLinkArray = Json["classToAbilityLinks"].AsArray;
				foreach (var classToAbilityLink in classToAbilityLinkArray)
				{
					JSONArray link = classToAbilityLink.Value.AsArray;
					abilityNodeList[link[1].AsInt].ClassNodeList.Add(classNodeList[link[0].AsInt]);
				}
				JSONArray kitsToAbilityLinkArray = Json["kitsToAbilityLinks"].AsArray;
				foreach (var kitsToAbilityLink in kitsToAbilityLinkArray)
				{
					JSONArray link = kitsToAbilityLink.Value.AsArray;
					abilityNodeList[link[1].AsInt].KitsNodeList.Add(kitNodeList[link[0].AsInt]);
				}
				
				//define the longer links between ability nodes
				foreach (var abilityNode in abilityNodeList)
				{
					abilityNode.DeepPopulateLinks(2);

					for (var i = 0; i < abilityNode.abilityNodeLinkList.Count; ++i)
					{
						if (abilityNode.abilityNodeLinkList[i] == null && i != abilityNode.Index)
						{
							Debug.Log("merp");
						}
					}
				}
			}

			private void PopulateNodes(JSONArray array_, Skill.TypeEnum type_, ref List<ConstellationNode> nodeList_)
			{
				foreach (var almostNode in array_)
				{
					JSONNode json = almostNode.Value;

					Skill skill;
					switch(type_)
					{
						case Skill.TypeEnum.Ability: skill = App.Content.AbilityList[json["id"]]; break;
						case Skill.TypeEnum.Class: skill = App.Content.ClassList[json["id"]]; break;
						case Skill.TypeEnum.Kit: skill = App.Content.KitList[json["id"]]; break;
						default: throw new Exception("PopulateNodes() with not type");
					}
					nodeList_.Add(new ConstellationNode(
						skill,
						nodeList_.Count, 
						new Vector2(json["position"]["x"].AsFloat, json["position"]["y"].AsFloat)));
				}
			}
		}
	}
}

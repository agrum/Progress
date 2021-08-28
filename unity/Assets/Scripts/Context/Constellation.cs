using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
	public class Constellation
	{
		public JSONNode Json { get; private set; } = null;
		public List<int> StartingAbilityNodeIndexList { get; private set; } = new List<int>();
		public List<ConstellationNode> AbilityNodeList { get; private set; } = new List<ConstellationNode>();
		public List<ConstellationNode> ClassNodeList { get; private set; } = new List<ConstellationNode>();
		public List<ConstellationNode> KitNodeList { get; private set; } = new List<ConstellationNode>();
		private Vector2 halfSize = new Vector2(0, 0);

		public ConstellationNode AbilityNode(Data.Skill.Skill skill)
		{
			foreach (var abilityNode in AbilityNodeList)
				if (abilityNode.Skill == skill)
					return abilityNode;

			return null;
		}

		public ConstellationNode ClassNode(Data.Skill.Skill skill)
		{
			foreach (var classNode in ClassNodeList)
				if (classNode.Skill == skill)
					return classNode;

			return null;
		}

		public ConstellationNode KitNode(Data.Skill.Skill skill)
		{
			foreach (var kitNode in KitNodeList)
				if (kitNode.Skill == skill)
					return kitNode;

			return null;
        }

        public Data.Skill.Skill Skill(string uuid)
        {
            foreach (var node in AbilityNodeList)
                if (node.Skill._Id.ToString() == uuid)
                    return node.Skill;
            foreach (var node in ClassNodeList)
                if (node.Skill._Id.ToString() == uuid)
                    return node.Skill;
            foreach (var node in KitNodeList)
                if (node.Skill._Id.ToString() == uuid)
                    return node.Skill;
            return null;
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
                Data.Skill.Skill.ECategory.Ability);
			PopulateNodes(
				Json["classes"].AsArray,
                Data.Skill.Skill.ECategory.Class);
			PopulateNodes(
				Json["kits"].AsArray,
                Data.Skill.Skill.ECategory.Kit);

			//connect nodes directly
			JSONArray abilityToAbilityLinkArray = Json["abilityToAbilityLinks"].AsArray;
			foreach (var abilityToAbilityLink in abilityToAbilityLinkArray)
			{
				JSONArray link = abilityToAbilityLink.Value.AsArray;
				new ConstellationNodeLink(AbilityNodeList[link[0].AsInt], AbilityNodeList[link[1].AsInt]);
			}
			JSONArray classToAbilityLinkArray = Json["classToAbilityLinks"].AsArray;
			foreach (var classToAbilityLink in classToAbilityLinkArray)
			{
				JSONArray link = classToAbilityLink.Value.AsArray;
				AbilityNodeList[link[1].AsInt].ClassNodeList.Add(ClassNodeList[link[0].AsInt]);
			}
			JSONArray kitsToAbilityLinkArray = Json["kitsToAbilityLinks"].AsArray;
			foreach (var kitsToAbilityLink in kitsToAbilityLinkArray)
			{
				JSONArray link = kitsToAbilityLink.Value.AsArray;
				AbilityNodeList[link[1].AsInt].KitsNodeList.Add(KitNodeList[link[0].AsInt]);
			}
				
			//define the longer links between ability nodes
			foreach (var abilityNode in AbilityNodeList)
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

		private void PopulateNodes(JSONArray array_, Data.Skill.Skill.ECategory category_)
		{
			foreach (var almostNode in array_)
			{
				JSONNode json = almostNode.Value;
                Debug.Log(json.ToString());

                switch (category_)
				{
					case Data.Skill.Skill.ECategory.Ability:
						AbilityNodeList.Add(new ConstellationNode(
							App.Content.SkillList.Abilities[json["id"]],
							AbilityNodeList.Count,
							new Vector2(json["position"]["x"].AsFloat, json["position"]["y"].AsFloat)));
						break;
                    case Data.Skill.Skill.ECategory.Class:
						ClassNodeList.Add(new ConstellationNode(
                            App.Content.SkillList.Classes[json["id"]],
                            ClassNodeList.Count,
							new Vector2(json["position"]["x"].AsFloat, json["position"]["y"].AsFloat)));
							break;
					case Data.Skill.Skill.ECategory.Kit:
						KitNodeList.Add(new ConstellationNode(
                            App.Content.SkillList.Kits[json["id"]],
                            KitNodeList.Count,
							new Vector2(json["position"]["x"].AsFloat, json["position"]["y"].AsFloat)));
							break;
					default: throw new Exception("PopulateNodes() with not type");
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;

namespace West
{
	namespace Model
	{
		public class ConstellationPreset
		{
            public JSONNode Json { get; private set; } = null;
			public string Name { get; private set; }
			public Constellation Constellation { get; private set; }

			public List<Skill> SelectedAbilityList { get; private set; } = new List<Skill>();
			public List<Skill> SelectedClassList { get; private set; } = new List<Skill>();
			public List<Skill> SelectedKitList { get; private set; } = new List<Skill>();

			public delegate void OnPresetUpdateDelegate();
			public event OnPresetUpdateDelegate presetUpdateEvent;

			public ConstellationPreset(JSONNode json)
			{
                Json = json;
                Name = json["name"];
				Constellation = App.Content.ConstellationList[json["constellation"]];

				foreach (var almostValue in json["abilities"])
					SelectedAbilityList.Add(App.Content.AbilityList[almostValue.Value]);
				foreach (var almostValue in json["classes"])
					SelectedClassList.Add(App.Content.ClassList[almostValue.Value]);
				foreach (var almostValue in json["kits"])
					SelectedKitList.Add(App.Content.KitList[almostValue.Value]);
			}

			public void Add(ConstellationNode node)
			{
				List<Skill> SelectedIndexList;
				int limit;

				if (node == null)
				{
					Debug.Log("ConstellationPreset.Add() node null");
					throw new Exception();
				}

				switch (node.Skill.Type)
				{
					case Skill.TypeEnum.Ability:
						SelectedIndexList = SelectedAbilityList;
						limit = App.Content.GameSettings.NumAbilities;
						break;
					case Skill.TypeEnum.Class:
						SelectedIndexList = SelectedClassList;
						limit = App.Content.GameSettings.NumClasses;
						break;
					case Skill.TypeEnum.Kit:
						SelectedIndexList = SelectedKitList;
						limit = App.Content.GameSettings.NumKits;
						break;
					default:
						Debug.Log("ConstellationPreset.Add() node no type");
						throw new Exception();
				}

				if (SelectedIndexList.Count >= limit)
				{
					Debug.Log("ConstellationPreset.Add() can't");
					throw new Exception();
				}

				SelectedIndexList.Add(node.Skill);
				presetUpdateEvent();
			}

			public void Remove(ConstellationNode node)
			{
				List<Skill> SelectedList;
				int limit;

				if (node == null)
				{
					Debug.Log("ConstellationPreset.Remove() node null");
					throw new Exception();
				}

				switch (node.Skill.Type)
				{
					case Skill.TypeEnum.Ability:
						SelectedList = SelectedAbilityList;
						limit = App.Content.GameSettings.NumAbilities;
						break;
					case Skill.TypeEnum.Class:
						SelectedList = SelectedClassList;
						limit = App.Content.GameSettings.NumClasses;
						break;
					case Skill.TypeEnum.Kit:
						SelectedList = SelectedKitList;
						limit = App.Content.GameSettings.NumKits;
						break;
					default:
						Debug.Log("ConstellationPreset.Remove() node no type");
						throw new Exception();
				}

				if (SelectedList.Count == 0 || !SelectedList.Contains(node.Skill))
				{
					Debug.Log("ConstellationPreset.Remove() can't");
					throw new Exception();
				}

				if (node.Skill.Type == Skill.TypeEnum.Ability)
				{
					//clear if preset doesn't contain a starting node
					bool hasStartingAbility = false;
					foreach (var startingAbilityNodeIndex in Constellation.StartingAbilityNodeIndexList)
					{
						Skill startingSkill = Constellation.AbilityNodeList[startingAbilityNodeIndex].Skill;
						if (SelectedAbilityList.Contains(startingSkill))
						{
							hasStartingAbility = true;
							break;
						}
					}
					if (!hasStartingAbility)
					{
						Clear();
						return;
					}

					SelectedAbilityList.Remove(node.Skill);
					//unselect classes and kits that were solely dependent on this ability
					var newSelectedClassList = new List<Skill>();
					foreach (var selectedClass in SelectedClassList)
					{
						var selectedClassNode = Constellation.ClassNode(selectedClass);
						foreach (var selectedAbility in SelectedAbilityList)
						{
							if (Constellation.AbilityNode(selectedAbility).ClassNodeList.Contains(selectedClassNode))
							{
								newSelectedClassList.Add(selectedClass);
								break;
							}
						}
					}
					SelectedClassList = newSelectedClassList;
					var newSelectedKitList = new List<Skill>();
					foreach (var selectedKit in SelectedKitList)
					{
						var selectedKitNode = Constellation.KitNode(selectedKit);
						foreach (var selectedAbility in SelectedAbilityList)
						{
							if (Constellation.AbilityNode(selectedAbility).KitsNodeList.Contains(selectedKitNode))
							{
								newSelectedKitList.Add(selectedKit);
								break;
							}
						}
					}
					SelectedKitList = newSelectedKitList;
				}
				else
					SelectedList.Remove(node.Skill);

				presetUpdateEvent();
			}

			public void Clear()
			{
				SelectedAbilityList.Clear();
				SelectedClassList.Clear();
				SelectedKitList.Clear();
				presetUpdateEvent();
			}
		}
	}
}

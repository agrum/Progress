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
			public string Name { get; private set; }
			public string Constellation { get; private set; }

			public List<int> SelectedAbilityIndexList { get; private set; } = new List<int>();
			public List<int> SelectedClassIndexList { get; private set; } = new List<int>();
			public List<int> SelectedKitIndexList { get; private set; } = new List<int>();

			public delegate void OnPresetUpdateDelegate();
			public event OnPresetUpdateDelegate presetUpdateEvent;

			public ConstellationPreset(JSONNode json)
			{
				Name = json["name"];
				Constellation = json["constellation"];

				foreach (var almostValue in json["selectedAbilityIndexList"])
					SelectedAbilityIndexList.Add(almostValue.Value);
				foreach (var almostValue in json["selectedClassIndexList"])
					SelectedClassIndexList.Add(almostValue.Value);
				foreach (var almostValue in json["selectedKitIndexList"])
					SelectedKitIndexList.Add(almostValue.Value);
			}

			public void Add(ConstellationNode node)
			{
				List<int> SelectedIndexList;
				int limit;

				if (node == null)
				{
					Debug.Log("ConstellationPreset.Add() node null");
					throw new Exception();
				}

				switch (node.Skill.Type)
				{
					case Skill.TypeEnum.Ability:
						SelectedIndexList = SelectedAbilityIndexList;
						limit = App.Content.GameSettings.NumAbilities;
						break;
					case Skill.TypeEnum.Class:
						SelectedIndexList = SelectedClassIndexList;
						limit = App.Content.GameSettings.NumClasses;
						break;
					case Skill.TypeEnum.Kit:
						SelectedIndexList = SelectedKitIndexList;
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

				SelectedIndexList.Add(node.Index);
				presetUpdateEvent();
			}

			public void Remove(ConstellationNode node)
			{
				List<int> SelectedIndexList;
				int limit;

				if (node == null)
				{
					Debug.Log("ConstellationPreset.Remove() node null");
					throw new Exception();
				}

				switch (node.Skill.Type)
				{
					case Skill.TypeEnum.Ability:
						SelectedIndexList = SelectedAbilityIndexList;
						limit = App.Content.GameSettings.NumAbilities;
						break;
					case Skill.TypeEnum.Class:
						SelectedIndexList = SelectedClassIndexList;
						limit = App.Content.GameSettings.NumClasses;
						break;
					case Skill.TypeEnum.Kit:
						SelectedIndexList = SelectedKitIndexList;
						limit = App.Content.GameSettings.NumKits;
						break;
					default:
						Debug.Log("ConstellationPreset.Remove() node no type");
						throw new Exception();
				}

				if (SelectedIndexList.Count == 0 || !SelectedIndexList.Contains(node.Index))
				{
					Debug.Log("ConstellationPreset.Remove() can't");
					throw new Exception();
				}

				if (node.Skill.Type == Skill.TypeEnum.Ability)
				{
					if (SelectedAbilityIndexList[0] == node.Index) //clear if it's the initial node
					{
						Clear();
						return;
					}
					else
					{
						SelectedAbilityIndexList.Remove(node.Index);
						//unselect classes and kits that were solely dependent on this ability
						var newSelectedClassIndexList = new List<int>();
						foreach (var selectedClassIndex in SelectedClassIndexList)
						{
							var selectedClassNode = App.Content.Constellation.Model.ClassNodeList[selectedClassIndex];
							foreach (var selectedAbilityIndex in SelectedAbilityIndexList)
							{
								if (App.Content.Constellation.Model.AbilityNodeList[selectedAbilityIndex].ClassNodeList.Contains(selectedClassNode))
								{
									newSelectedClassIndexList.Add(selectedClassIndex);
									break;
								}
							}
						}
						SelectedClassIndexList = newSelectedClassIndexList;
						var newSelectedKitIndexList = new List<int>();
						foreach (var selectedKitIndex in SelectedKitIndexList)
						{
							var selectedKitNode = App.Content.Constellation.Model.KitNodeList[selectedKitIndex];
							foreach (var selectedAbilityIndex in SelectedAbilityIndexList)
							{
								if (App.Content.Constellation.Model.AbilityNodeList[selectedAbilityIndex].KitsNodeList.Contains(selectedKitNode))
								{
									newSelectedKitIndexList.Add(selectedKitIndex);
									break;
								}
							}
						}
						SelectedKitIndexList = newSelectedKitIndexList;
					}
				}
				else
					SelectedIndexList.Remove(node.Index);

				presetUpdateEvent();
			}

			public void Clear()
			{
				SelectedAbilityIndexList.Clear();
				SelectedClassIndexList.Clear();
				SelectedKitIndexList.Clear();
				presetUpdateEvent();
			}
		}
	}
}

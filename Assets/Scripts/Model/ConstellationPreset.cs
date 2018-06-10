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
            public string Id { get; private set; }
			public string Name { get; set; }
			public Constellation Constellation { get; private set; }

			public List<Skill> SelectedAbilityList { get; private set; } = new List<Skill>();
			public List<Skill> SelectedClassList { get; private set; } = new List<Skill>();
			public List<Skill> SelectedKitList { get; private set; } = new List<Skill>();

			public delegate void OnPresetUpdateDelegate();
			public event OnPresetUpdateDelegate presetUpdateEvent;

			public ConstellationPreset(JSONNode json)
			{
                Id = json["_id"];
                Name = json["name"];
				Constellation = App.Content.ConstellationList[json["constellation"]];

				foreach (var almostValue in json["abilities"])
					SelectedAbilityList.Add(App.Content.AbilityList[almostValue.Value]);
				foreach (var almostValue in json["classes"])
					SelectedClassList.Add(App.Content.ClassList[almostValue.Value]);
				foreach (var almostValue in json["kits"])
					SelectedKitList.Add(App.Content.KitList[almostValue.Value]);
			}

			public void Add(Skill skill)
			{
				List<Skill> SelectedIndexList;
				int limit;

				if (skill == null)
				{
					Debug.Log("ConstellationPreset.Add() skill null");
					throw new Exception();
				}

				switch (skill.Type)
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
						Debug.Log("ConstellationPreset.Add() skill no type");
						throw new Exception();
				}

				if (SelectedIndexList.Count >= limit)
				{
					Debug.Log("ConstellationPreset.Add() can't");
					throw new Exception();
				}

				SelectedIndexList.Add(skill);
				presetUpdateEvent();
			}

			public void Remove(Skill skill)
			{
				List<Skill> SelectedList;
				int limit;

				if (skill == null)
				{
					Debug.Log("ConstellationPreset.Remove() skill null");
					throw new Exception();
				}

				switch (skill.Type)
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
						Debug.Log("ConstellationPreset.Remove() skill no type");
						throw new Exception();
				}

				if (SelectedList.Count == 0 || !SelectedList.Contains(skill))
				{
					Debug.Log("ConstellationPreset.Remove() can't");
					throw new Exception();
				}

				if (skill.Type == Skill.TypeEnum.Ability)
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

					SelectedAbilityList.Remove(skill);
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
					SelectedList.Remove(skill);

				presetUpdateEvent();
			}

			public bool Has(Skill skill)
			{
				List<Skill> SelectedList;
				int limit;

				if (skill == null)
				{
					Debug.Log("ConstellationPreset.Has() skill null");
					throw new Exception();
				}

				switch (skill.Type)
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
						Debug.Log("ConstellationPreset.Has() skill no type");
						throw new Exception();
				}

				return SelectedList.Contains(skill);
			}

			public void Clear()
			{
				SelectedAbilityList.Clear();
				SelectedClassList.Clear();
				SelectedKitList.Clear();
				presetUpdateEvent();
			}

            public JSONNode ToJson()
            {
                JSONNode json = new JSONObject();

                json["_id"] = Id;
                json["name"] = Name;
                json["constellation"] = Constellation.Json["_id"];
                json["abilities"] = new JSONArray();
                json["classes"] = new JSONArray();
                json["kits"] = new JSONArray();
                foreach (var skill in SelectedAbilityList)
                    json["abilities"].Add(skill.Json["_id"]);
                foreach (var skill in SelectedClassList)
                    json["classes"].Add(skill.Json["_id"]);
                foreach (var skill in SelectedKitList)
                    json["kits"].Add(skill.Json["_id"]);

                return json;
            }
		}
	}
}

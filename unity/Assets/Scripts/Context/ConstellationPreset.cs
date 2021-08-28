using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class PresetLimits
    {
        public int Ability { get; private set; } = 0;
        public int Class { get; private set; } = 0;
        public int Kit { get; private set; } = 0;

        public PresetLimits(int ability_, int class_, int kit_)
        {
            Ability = ability_;
            Class = class_;
            Kit = kit_;
        }
    }

	public class ConstellationPreset
	{
        public string Id { get { return Json["_id"]; } private set { Json["_id"] = value; } }
		public string Name { get { return Json["name"]; } set { Json["name"] = value; } }
        public Constellation Constellation { get; private set; }
        public PresetLimits Limits { get; protected set; } = null;

        public List<Data.Skill.Skill> SelectedAbilityList { get; private set; } = new List<Data.Skill.Skill>();
		public List<Data.Skill.Skill> SelectedClassList { get; private set; } = new List<Data.Skill.Skill>();
		public List<Data.Skill.Skill> SelectedKitList { get; private set; } = new List<Data.Skill.Skill>();

		public delegate void OnPresetUpdateDelegate();
		public event OnPresetUpdateDelegate PresetUpdated;

        private JSONObject Json = null;

        public ConstellationPreset(JSONObject json_, PresetLimits limits_)
		{
            if (json_ == null)
            {
                json_ = new JSONObject();
            }

            Json = json_;
            Constellation = App.Content.ConstellationList[App.Content.GameSettings.Json["constellation"]];
            //Constellation = App.Content.ConstellationList[json["constellation"]];
            Limits = limits_;

            foreach (var almostValue in Json["abilities"])
				SelectedAbilityList.Add(App.Content.SkillList.Abilities[almostValue.Value]);
			foreach (var almostValue in Json["classes"])
				SelectedClassList.Add(App.Content.SkillList.Classes[almostValue.Value]);
			foreach (var almostValue in Json["kits"])
				SelectedKitList.Add(App.Content.SkillList.Kits[almostValue.Value]);
		}

		public void Add(Data.Skill.Skill skill)
		{
			List<Data.Skill.Skill> SelectedIndexList;
			int limit;

			if (skill == null)
			{
				Debug.Log("ConstellationPreset.Add() skill null");
				throw new Exception();
			}

			switch (skill.Category)
			{
				case Data.Skill.Skill.ECategory.Ability:
					SelectedIndexList = SelectedAbilityList;
					limit = Limits.Ability;
					break;
				case Data.Skill.Skill.ECategory.Class:
					SelectedIndexList = SelectedClassList;
					limit = Limits.Class;
					break;
				case Data.Skill.Skill.ECategory.Kit:
					SelectedIndexList = SelectedKitList;
                    limit = Limits.Kit; //App.Content.GameSettings.NumKits;
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
			PresetUpdated();
		}

		public void Remove(Data.Skill.Skill skill)
		{
			List<Data.Skill.Skill> SelectedList;
			int limit;

			if (skill == null)
			{
				Debug.Log("ConstellationPreset.Remove() skill null");
				throw new Exception();
			}

			switch (skill.Category)
			{
				case Data.Skill.Skill.ECategory.Ability:
					SelectedList = SelectedAbilityList;
					limit = App.Content.GameSettings.NumAbilities;
					break;
				case Data.Skill.Skill.ECategory.Class:
					SelectedList = SelectedClassList;
					limit = App.Content.GameSettings.NumClasses;
					break;
				case Data.Skill.Skill.ECategory.Kit:
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

			/*if (skill.Type == Skill.TypeEnum.Ability)
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
			else*/
				SelectedList.Remove(skill);

			PresetUpdated();
		}

		public bool Has(Data.Skill.Skill skill)
		{
			List<Data.Skill.Skill> SelectedList;
			int limit;

			if (skill == null)
			{
				Debug.Log("ConstellationPreset.Has() skill null");
				throw new Exception();
			}

			switch (skill.Category)
			{
				case Data.Skill.Skill.ECategory.Ability:
					SelectedList = SelectedAbilityList;
					limit = App.Content.GameSettings.NumAbilities;
					break;
				case Data.Skill.Skill.ECategory.Class:
					SelectedList = SelectedClassList;
					limit = App.Content.GameSettings.NumClasses;
					break;
				case Data.Skill.Skill.ECategory.Kit:
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
			PresetUpdated();
		}

        public static implicit operator JSONObject(ConstellationPreset object_)
        {
            JSONObject json = new JSONObject();

            if (object_.Id != null)
                json["_id"] = object_.Id;
            json["name"] = object_?.Name ?? "";
            json["constellation"] = object_.Constellation.Json["_id"];
            json["abilities"] = new JSONArray();
            json["classes"] = new JSONArray();
            json["kits"] = new JSONArray();
            foreach (var skill in object_.SelectedAbilityList)
                json["abilities"].Add(skill._Id.ToString());
            foreach (var skill in object_.SelectedClassList)
                json["classes"].Add(skill._Id.ToString());
            foreach (var skill in object_.SelectedKitList)
                json["kits"].Add(skill._Id.ToString());
            
            return json;
        }
	}
}

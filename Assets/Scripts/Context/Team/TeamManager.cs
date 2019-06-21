using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class TeamManager
	{
		//static instantiation
		private static TeamManager _instance;
		public static TeamManager Instance
		{
			get
			{
				if (_instance == null)
					_instance = new TeamManager();
				return _instance;
			}
		}
		//static instantiation

		private List<Team> teamList = new List<Team>();

		private TeamManager()
		{
			//Assets.Scripts.App.Content.GameSettings.Load(() =>
			//{
			//	int numTeams = Assets.Scripts.App.Content.GameSettings.Json["numberOfTeams"].AsInt;

			//	for (int i = 0; i < numTeams; i++)
			//		teamList.Add(new Team(Assets.Scripts.App.Content.GameSettings.Json["numberOfPlayersPerTeam"].AsInt));

			//	NexusManager.Instance.AllocateMexusesBetweenTeams(ref teamList);
			//});
		}

		public List<Team> TeamList
		{
			get
			{
				return teamList;
			}
		}
	}
}
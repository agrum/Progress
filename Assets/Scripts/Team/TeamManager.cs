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
			App.Load(() =>
			{
				int numTeams = App.Model["numberOfTeams"].AsInt;

				for (int i = 0; i < numTeams; i++)
					teamList.Add(new Team(App.Model["numberOfPlayersPerTeam"].AsInt));

				NexusManager.Instance.AllocateMexusesBetweenTeams(ref teamList);
			});
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
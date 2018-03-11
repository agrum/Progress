using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private List<Team> teamList;

	private TeamManager()
	{
		uint numTeams = 3;

		for(int i = 0; i < numTeams; i++)
			teamList.Add(new Team());

		NexusManager.Instance.AllocateMexusesBetweenTeams(ref teamList);
	}

	public List<Team> TeamList
	{
		get
		{
			return teamList;
		}
	}
}

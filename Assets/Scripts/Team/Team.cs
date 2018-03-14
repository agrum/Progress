using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team {
	private List<Player> playerList = new List<Player>();

	public Team(int size)
	{
		while (size > playerList.Count)
			playerList.Add(null);
	}
	
	public Player this[int i]
	{
		get
		{
			return playerList[i];
		}
		set
		{
			playerList[i] = value;
		}
	}

	public int TeamSize()
	{
		return playerList.Count;
	}
}

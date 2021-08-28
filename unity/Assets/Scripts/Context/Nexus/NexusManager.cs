using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class NexusManager
	{
		//static instantiation
		private static NexusManager _instance;
		public static NexusManager Instance
		{
			get
			{
				if (_instance == null)
					_instance = new NexusManager();
				return _instance;
			}
		}
		//static instantiation
		private Nexus[] NexusArray;

		private NexusManager()
		{
			NexusArray = Object.FindObjectsOfType<Nexus>();
		}

		public void AllocateMexusesBetweenTeams(ref List<Team> teamList)
		{
			//find starting nexuses
			List<Nexus> startingNexusList = new List<Nexus>();
			foreach (Nexus nexus in NexusArray)
			{
				nexus.Team = null;
				startingNexusList.Add(nexus);
			}

			//Only keep the right amount of starting nexus to split equally among teams
			int fairShareNexusNumber = (startingNexusList.Count / teamList.Count) * teamList.Count;
			while (startingNexusList.Count > fairShareNexusNumber)
				startingNexusList.RemoveAt((int)(Random.Range(0, 0.999f) * startingNexusList.Count));

			//give away nexuses randomly
			int allocatedCount = 0;
			while (startingNexusList.Count > 0)
			{
				int index = (int)(Random.Range(0, 0.999f) * startingNexusList.Count);
				startingNexusList[index].Team = teamList[allocatedCount++ % teamList.Count];
				startingNexusList.RemoveAt(index);
			}
		}
	}
}
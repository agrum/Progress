using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace West
{
	public class Nexus : MonoBehaviour
	{

		public float fullPowerRadius;
		public float maxRangeRadius;
		public List<Nexus> neighbourNexusList;
		public bool startingNexus;

		private Team team;

		public Team Team
		{
			get
			{
				return team;
			}
			set
			{
				team = value;
			}
		}
	}
}
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace West
{
	[CustomEditor(typeof(Nexus))]
	public class NexusEditor : Editor
	{
		void OnSceneGUI()
		{
			Nexus nexus = (Nexus)target;
			Handles.color = Color.white;
			Handles.DrawWireArc(nexus.transform.position, Vector3.up, Vector3.forward, 360, nexus.fullPowerRadius);
			Handles.DrawWireArc(nexus.transform.position, Vector3.up, Vector3.forward, 360, nexus.maxRangeRadius);
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceAffinityVisual : MonoBehaviour {
	public Player player;

	// Use this for initialization
	void Start () {
		transform.Rotate(0, 0, 360.0f * (0.5f - player.resourceAffinity));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

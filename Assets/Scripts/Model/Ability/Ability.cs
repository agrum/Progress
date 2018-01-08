using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Ability : MonoBehaviour
{
	public String keybind { get; set; }
	public Player player;

	virtual public String Info()
	{
		return "";
	}

	virtual public void Activate()
	{

	}

	virtual public void Cancel()
	{

	}

	virtual public bool CanActivate()
	{
		return true;
	}
}


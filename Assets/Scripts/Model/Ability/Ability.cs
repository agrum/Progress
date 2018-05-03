using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace West
{
	public class Ability : MonoBehaviour
	{
		public Player player { get; internal set; }
		public string keybind { get; internal set; }

		virtual public string Info()
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
}
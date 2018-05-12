using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace West
{
	namespace View
	{
		class WestBehaviour : MonoBehaviour
		{
			protected delegate void OnStartedDelegate();

			protected bool started = false;
			protected OnStartedDelegate setupDelegate = null;

			protected virtual void Start()
			{
				setupDelegate?.Invoke();
				started = true;
			}

			protected void SetupOnStarted(OnStartedDelegate setupDelegate_)
			{
				if (started)
					setupDelegate_?.Invoke();
				else
					setupDelegate = setupDelegate_;
			}
		}
	}
}

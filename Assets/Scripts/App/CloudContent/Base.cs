using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace West.Model.CloudContent
{
	public abstract class Base
	{
		public delegate void OnLoaded();
		public delegate void OnBuilt();
		public event OnLoaded LoadedEvent;
				
		public void Load(OnLoaded onLoaded_)
		{
			//callback if already loaded.
			if (loaded)
			{
				onLoaded_();
				return;
			}

			//bind to event
			LoadedEvent += onLoaded_;

			//return if already loading
			if (loading)
				return;

			//set as loading 
			loading = true;

			//check if we need to load the dependencies.
			bool dependencies_loaded = true;
			foreach (var dependency in dependencyList)
				if (!dependency.loaded)
				{
					dependencies_loaded = false;
					break;
				}
					
			//define all dependency acquired callback
			OnBuilt selfBuiltCallback = () =>
			{
				loaded = true;
				loading = false;
				Debug.Log("Cloud content : " + this.GetType().Name + " loaded");
				LoadedEvent();
			};

			//if no dependencies needed, just build yourself directly
			if (dependencies_loaded)
			{
				Build(selfBuiltCallback);
			}

			//otherwise define per-dependency-built callback
			int dependency_needed_count = dependencyList.Count;
			OnLoaded dependencyBuiltCallback = () =>
			{
				Interlocked.Decrement(ref dependency_needed_count);

				//when all requests have been processed, call async callback's
				if (Interlocked.Equals(dependency_needed_count, 0))
						Build(selfBuiltCallback);
			};

			//and load all dependencies, then trigger self build
			foreach (var dependency in dependencyList)
			{
				dependency.Load(dependencyBuiltCallback);
			}
		}

		protected abstract void Build(OnBuilt onBuilt_);

		protected List<Base> dependencyList = new List<Base>();
		protected bool loaded = false;
		protected bool loading = false;
	}
}
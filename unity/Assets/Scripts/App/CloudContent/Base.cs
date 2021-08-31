using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.CloudContent
{
	public abstract class Base
	{
		public delegate void OnLoaded();
		public delegate void OnBuilt();
		public event OnLoaded LoadedEvent;
        public static GameObject LoadingScreen = null;
        public static uint LoadCount = 0;

        public IEnumerator Load()
        {
            //return if already loading
            while (loading)
            {
                yield return new WaitForSeconds(0);
            }

            //callback if already loaded.
            if (loaded)
            {
                yield break;
            }

            //set as loading 
            loading = true;
            LoadCount++;
            LoadingScreen = LoadingScreen ?? App.Resource.Prefab.LoadingCanvas();

            //check if we need to load the dependencies.
            bool dependencies_loaded = true;
            foreach (var dependency in dependencyList)
            {
                if (!dependency.loaded)
                {
                    dependencies_loaded = false;
                    break;
                }
            }

            //if no dependencies needed, just build yourself directly
            if (dependencies_loaded)
            {
                yield return Build();
                Loaded();
                yield break;
            }

            //otherwise define per-dependency-built callback
            int dependency_needed_count = dependencyList.Count;

            //and load all dependencies, then trigger self build
            foreach (var dependency in dependencyList)
            {
                yield return dependency.Load();
                Interlocked.Decrement(ref dependency_needed_count);

                //when all requests have been processed, call async callback's
                if (Interlocked.Equals(dependency_needed_count, 0))
                {
                    yield return Build();
                    Loaded();
                    yield break;
                }
            }
        }

        public void Unload()
        {
            loaded = false;
            loading = false;
        }

        private void Loaded()
        {
            loaded = true;
            loading = false;
            LoadCount--;
            if (LoadCount == 0)
            {
                GameObject.Destroy(LoadingScreen);
                LoadingScreen = null;
            }
            Debug.Log("Cloud content : " + this.GetType().Name + " loaded");
        }
        
        protected virtual IEnumerator Build() { return null; }

        protected List<Base> dependencyList = new List<Base>();
		protected bool loaded = false;
		protected bool loading = false;
	}
}
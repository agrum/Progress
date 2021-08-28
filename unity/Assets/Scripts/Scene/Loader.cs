using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scene
{
    public class Loader
    {
        public void Load(string name)
        {
            App.Resource.Prefab.LoadingCanvas();
            SceneManager.LoadScene(name);
        }
    }
}

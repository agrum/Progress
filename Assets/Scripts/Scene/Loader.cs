using UnityEngine.SceneManagement;

namespace West.Model
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

using UnityEngine;
using UnityEngine.SceneManagement;

namespace JJM.Scripts
{
    public class SceneChangeEvent : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        public void SceneChange()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
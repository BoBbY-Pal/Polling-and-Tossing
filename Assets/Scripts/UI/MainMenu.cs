using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1); 
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

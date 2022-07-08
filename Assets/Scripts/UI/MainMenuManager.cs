using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SoundManager.Instance.Play(Sounds.ButtonClick);
            SceneManager.LoadScene(sceneBuildIndex: 1); 
        }
        public void QuitGame()
        {
            SoundManager.Instance.Play(Sounds.ButtonClick);
            Application.Quit();
        }

        public void GoToMainMenu()
        {
            SoundManager.Instance.Play(Sounds.ButtonClick);
            SceneManager.LoadScene("MainMenu");
        }
    }
}

using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            SceneManager.LoadScene(sceneBuildIndex: 1); 
        }
        public void QuitGame()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            Application.Quit();
        }

        public void GoToMainMenu()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            SceneManager.LoadScene("MainMenu");
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
    
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public TMP_Text timeText;
        public TMP_Text scoreText;

        public TMP_Text finalScore;
        public TMP_Text congoText;
        public GameObject[] stars;
    
        public GameObject roundOverScreen;
        public GameObject pauseScreen;
    
        private void Start()
        {
            foreach (GameObject t in stars)
            {
                t.SetActive(false);
            }
        }
  
        public void PauseUnpauseScreen()
        {
            if (!pauseScreen.activeInHierarchy)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }

        }

        public void ShuffleBoard()
        {
            GameManager.Instance.ShuffleBoard();
        }

        public void GoToLevelSelect()
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            SceneManager.LoadScene("LevelSelection");
        }
        public void ExitGame()
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }

        public void TryAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            roundOverScreen.SetActive(false);
        }
    }
}

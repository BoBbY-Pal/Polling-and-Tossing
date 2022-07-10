using System;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public TMP_Text timeText;
        public TMP_Text scoreText;

        public TMP_Text finalScore;
        public TMP_Text congoText;
        public GameObject[] stars;
    
        public GameObject roundOverScreen;
        public GameObject pauseScreen;
    
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
        private void Start()
        {
            foreach (GameObject t in stars)
            {
                t.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnpauseScreen();
            }
        }

        public void PauseUnpauseScreen()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            
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
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            GameManager.Instance.ShuffleBoard();
        }

        public void GoToLevelSelect()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            SceneManager.LoadScene("LevelSelection");
        }
        public void ExitGame()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            SceneManager.LoadScene("MainMenu");
        }

        public void TryAgain()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            roundOverScreen.SetActive(false);
        }
    }
}

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
    
        [Tooltip("Amount of time in seconds after which round will end.")]
        public float roundTime = 60f;
        private bool b_RoundEnd;
        
        private void Awake()
        {
            #region SINGLETON   
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            #endregion
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
            StartRoundTimer();
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnpauseScreen();
            }
        }
        
        private void StartRoundTimer()
        {
            if (roundTime > 0)
            {
                roundTime -= Time.deltaTime;

                if (roundTime <= 0)
                {
                    roundTime = 0;
                    b_RoundEnd = true;
                }
            }

            if (b_RoundEnd && GameManager.Instance.currenState == BoardState.Move)
            {
                RoundOver();
                b_RoundEnd = false;
            }

            timeText.text = roundTime.ToString("0.0") + "s";
        }

        private void RoundOver()
        {
            roundOverScreen.SetActive(true);
    
            finalScore.text = ScoreManager.Instance.CurrentScore.ToString();
        
            SoundManager.Instance.Play(SoundTypes.RoundOver);

            ScoreManager.Instance.GiveRating();  
        
            DisplayRating();
        }
        
        private void DisplayRating()
        {
            // Show how many stars user earned while playing game.
            for (int i = 0; i < ScoreManager.Instance.EarnedStars; i++)
            {
                stars[i].SetActive(true);
            }
        }

        public void ShuffleBoard()
        {
            SoundManager.Instance.Play(SoundTypes.ButtonClick);
            GameManager.Instance.ShuffleBoard();
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

using Enums;
using Managers;
using ScriptableObject;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private TargetScoreSO targetScoreSO;
    
    [HideInInspector]
    public float roundTime = 60f;
    private bool b_RoundEnd;

    [HideInInspector]
    public int currentScore;        // Current score of the game.
    private float displayScore;     // Score that fill be displayed on the screen.
    
    [Tooltip("How fast score changes")]
    public int scoreTransitionSpeed;

    private int earnedStars;
    private int prevEarnedStars;
    
    private int star1scoreTarget, star2scoreTarget, star3scoreTarget;

    private void Awake()
    {
        GetTargetScores();
    }

    private void GetTargetScores()
    {
        roundTime = targetScoreSO.roundTime;
        star1scoreTarget = targetScoreSO.star1ScoreTarget;
        star2scoreTarget = targetScoreSO.star2ScoreTarget;
        star3scoreTarget = targetScoreSO.star3ScoreTarget;
    }

    void Update()
    {
        StartRoundTimer();

        DisplayScore();
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

        UIManager.Instance.timeText.text = roundTime.ToString("0.0") + "s";
    }

    private void DisplayScore()
    {
        displayScore = Mathf.Lerp(displayScore, currentScore, scoreTransitionSpeed * Time.deltaTime);
        UIManager.Instance.scoreText.text = displayScore.ToString("0");
    }

    private void RoundOver()
    {
        UIManager.Instance.roundOverScreen.SetActive(true);
    
        UIManager.Instance.finalScore.text = currentScore.ToString();
        
        SoundManager.Instance.Play(SoundTypes.RoundOver);

        GiveRating();  
        
        DisplayRating();
    }
    
    private void GiveRating()
    {
        if (currentScore >= star1scoreTarget && currentScore < star2scoreTarget)
        {
            earnedStars = 1;
            UIManager.Instance.congoText.text = "Congratulations you earned " + earnedStars + " star!";
        }

        else if (currentScore >= star2scoreTarget && currentScore < star3scoreTarget)
        {
            earnedStars = 2;
            UIManager.Instance.congoText.text = "Congratulations you earned " + earnedStars + " stars!";
        }

        else if (currentScore >= star3scoreTarget)
        {
            earnedStars = 3;
            UIManager.Instance.congoText.text = "Congratulations you earned " + earnedStars + " stars!";
        }
        else
        {
            earnedStars = 0;
            UIManager.Instance.congoText.text = "Oh no! No stars for you, Wanna try again?";
        }
    }
   
    private void DisplayRating()
    {
        // Fetching how many stars user earned while playing in the past. 
        prevEarnedStars = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "StarsEarned");

        //Storing earned stars from current match in a local disk. 
        if (earnedStars > prevEarnedStars)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "StarsEarned", earnedStars);
        }

        // Show how many stars user earned while playing game.
        for (int i = 0; i < earnedStars; i++)
        {
            UIManager.Instance.stars[i].SetActive(true);
        }
        
        //  Check whether user completed the level or not..
        if (earnedStars >= 2)
        {
            LevelManager.Instance.MarkLevelComplete();
        }
    }
}

using System;
using Managers;
using ScriptableObject;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoGenericSingleton<ScoreManager>
{
    [SerializeField]
    private TargetScoreSO targetScoreSO;
    private int m_star1scoreTarget, m_star2scoreTarget, m_star3scoreTarget;
    
    [HideInInspector]
    public int scoreMultiplierCount;
    private float m_bonusAmount = .5f;
    
    public int CurrentScore { get; private set; } // Current score of the game.
    private float m_displayScore;     // Score that fill be displayed on the screen.
    
    [Tooltip("How fast score changes")]
    public int scoreTransitionSpeed;

    public int EarnedStars { get; private set; }
    private int m_prevEarnedStars;

    private void OnEnable()
    {
        Gem.GemDestroyed += UpdateScore;
    }
    
    private void OnDisable()
    {
        Gem.GemDestroyed -= UpdateScore;
    }

    protected override void Awake()
    {
        base.Awake();
            
        GetTargetScores();
    }

    private void Update()
    {
        DisplayScore();
    }

    private void GetTargetScores()
    {
        m_star1scoreTarget = targetScoreSO.star1ScoreTarget;
        m_star2scoreTarget = targetScoreSO.star2ScoreTarget;
        m_star3scoreTarget = targetScoreSO.star3ScoreTarget;
    }

    private void UpdateScore(Gem gemToCheck)
    {
        CurrentScore += gemToCheck.scoreValue;
        
        // Bonus score
        if (scoreMultiplierCount > 0)
        {
            float bonusToAdd = gemToCheck.scoreValue * scoreMultiplierCount * m_bonusAmount;
            CurrentScore += Mathf.RoundToInt(bonusToAdd);
        }
    }
    
    private void DisplayScore()
    {
        m_displayScore = Mathf.Lerp(m_displayScore, CurrentScore, scoreTransitionSpeed * Time.deltaTime);
        UIManager.Instance.scoreText.text = m_displayScore.ToString("0");
    }
    
    public void GiveRating()
    {
        if (CurrentScore >= m_star1scoreTarget && CurrentScore < m_star2scoreTarget)
        {
            EarnedStars = 1;
            UIManager.Instance.congoText.text = "Congratulations you earned " + EarnedStars + " star!";
        }

        else if (CurrentScore >= m_star2scoreTarget && CurrentScore < m_star3scoreTarget)
        {
            EarnedStars = 2;
            UIManager.Instance.congoText.text = "Congratulations you earned " + EarnedStars + " stars!";
        }

        else if (CurrentScore >= m_star3scoreTarget)
        {
            EarnedStars = 3;
            UIManager.Instance.congoText.text = "Congratulations you earned " + EarnedStars + " stars!";
        }
        else
        {
            EarnedStars = 0;
            UIManager.Instance.congoText.text = "Oh no! No stars for you, Wanna try again?";
        }
        
        // Fetching how many stars user earned while playing in the past. 
        m_prevEarnedStars = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "StarsEarned");
        
        //Storing earned stars from current match in a local disk. 
        if (EarnedStars > m_prevEarnedStars)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "StarsEarned", EarnedStars);
        }
        
        //  Check whether user completed the level or not..
        if (EarnedStars >= 2)
        {
            LevelManager.Instance.MarkLevelComplete();
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager _uiManager;

    private bool b_RoundEnd;
    public BoardManager board;

    [HideInInspector] public int currentScore;
    private float displayScore;
    public int scoreTransitionSpeed;

    private int starsEarned;
    
    [Header("Score Targets")]
    [Tooltip("When player hits target score player will get stars according to that.")]
    [SerializeField] private int scoreTarget1Star, scoreTarget2Star, scoreTarget3Star;
    
    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
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

        if (b_RoundEnd && board.currenState == BoardState.Move)
        {
            WinCheck();
            b_RoundEnd = false;
        }

        _uiManager.timeText.text = roundTime.ToString("0.0") + "s";
    }

    private void DisplayScore()
    {
        displayScore = Mathf.Lerp(displayScore, currentScore, scoreTransitionSpeed * Time.deltaTime);
        _uiManager.scoreText.text = displayScore.ToString("0");
    }

    private void WinCheck()
    {
        _uiManager.roundOverScreen.SetActive(true);

        _uiManager.finalScore.text = currentScore.ToString();

        if (currentScore >= scoreTarget1Star && currentScore < scoreTarget2Star)
        {
             starsEarned = 1;
            _uiManager.congoText.text = "Congratulations you earned " + starsEarned + " star!";
        }

        else if ( currentScore >= scoreTarget2Star && currentScore < scoreTarget3Star)
        {
            starsEarned = 2;
            _uiManager.congoText.text = "Congratulations you earned " + starsEarned + " stars!";
            
        }

        else if (currentScore >= scoreTarget3Star)
        {
            starsEarned = 3;
            _uiManager.congoText.text = "Congratulations you earned " + starsEarned + " stars!";
        
        }
        else
        {
            starsEarned = 0;
            _uiManager.congoText.text = "Oh no! No stars for you, Wanna try again?";
        }
        
        for (int i = 0; i < starsEarned; i++)
        {
            _uiManager.stars[i].SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;

    private bool b_RoundEnd;
    public BoardManager board;

    //[HideInInspector]
    public int currentScore;
    private float displayScore;
    public int scoreTransitionSpeed;

    private int starsEarned;
    
    [Header("Score Targets")]
    [Tooltip("When player hits target score player will get stars according to that.")]
    [SerializeField] private int scoreTarget1Star, scoreTarget2Star, scoreTarget3Star;

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

        UIManager.Instance.timeText.text = roundTime.ToString("0.0") + "s";
    }

    private void DisplayScore()
    {
        displayScore = Mathf.Lerp(displayScore, currentScore, scoreTransitionSpeed * Time.deltaTime);
        UIManager.Instance.scoreText.text = displayScore.ToString("0");
    }

    private void WinCheck()
    {
        UIManager.Instance.roundOverScreen.SetActive(true);

        UIManager.Instance.finalScore.text = currentScore.ToString();

        if (currentScore >= scoreTarget1Star && currentScore < scoreTarget2Star)
        {
             starsEarned = 1;
            UIManager.Instance.congoText.text = "Congratulations you earned " + starsEarned + " star!";
        }

        else if ( currentScore >= scoreTarget2Star && currentScore < scoreTarget3Star)
        {
            starsEarned = 2;
            UIManager.Instance.congoText.text = "Congratulations you earned " + starsEarned + " stars!";
            
        }

        else if (currentScore >= scoreTarget3Star)
        {
            starsEarned = 3;
            UIManager.Instance.congoText.text = "Congratulations you earned " + starsEarned + " stars!";
        
        }
        else
        {
            starsEarned = 0;
            UIManager.Instance.congoText.text = "Oh no! No stars for you, Wanna try again?";
        }
        //Storing earned stars in a local disk. 
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "StarsEarned", starsEarned);
        
        SFXManager.Instance.PlayRoundOverSound();
        
        // Show how many stars user earned while playing game.
        for (int i = 0; i < starsEarned; i++)
        {   
            UIManager.Instance.stars[i].SetActive(true);
            
        }
    }
}

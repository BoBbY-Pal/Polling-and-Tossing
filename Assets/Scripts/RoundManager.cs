using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager _uiManager;

    private bool endingRound = false;
    public BoardManager board;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if (roundTime <= 0)
            {
                roundTime = 0;
                endingRound = true;
            }
        }

        if (endingRound && board.currenState == BoardState.Move)
        {
            WinCheck();
            endingRound = false;
        }
        
        _uiManager.timeText.text = roundTime.ToString("0.0") + "s";
    }

    private void WinCheck()
    {
        _uiManager.roundOverScreen.SetActive(true);
    }
}

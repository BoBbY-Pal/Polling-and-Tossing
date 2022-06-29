using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;

    public TMP_Text finalScore;
    public TMP_Text congoText;
    public GameObject[] stars;
    
    public GameObject roundOverScreen;
    // private RoundManager _roundManager;

    private void Start()
    {
        foreach (GameObject t in stars)
        {
            t.SetActive(false);
        }
    }
}

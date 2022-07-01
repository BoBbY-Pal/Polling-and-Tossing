using Singleton;
using TMPro;
using UnityEngine;

public class UIManager : MonoGenericSingleton<UIManager>
{
    public TMP_Text timeText;
    public TMP_Text scoreText;

    public TMP_Text finalScore;
    public TMP_Text congoText;
    public GameObject[] stars;
    
    public GameObject roundOverScreen;

    private void Start()
    {
        foreach (GameObject t in stars)
        {
            t.SetActive(false);
        }
    }
}

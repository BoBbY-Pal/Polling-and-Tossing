using System.Collections;
using Enums;
using Managers;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour 
{
    public string levelToLoad;
    public GameObject[] stars;

    public GameObject levelLockedWarning;
    
    private void Start()
    {
        int starsEarned = PlayerPrefs.GetInt(levelToLoad + "StarsEarned");

        for (int i = 0; i < starsEarned; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void LoadLevel()
    {   
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelToLoad);
        switch (levelStatus) {
            
            case LevelStatus.Locked:
                SoundManager.Instance.Play(SoundTypes.ButtonClick);
                levelLockedWarning.SetActive(true);
                
                StartCoroutine(DeactivateLevelLockedScreen());
                break;
            case LevelStatus.Unlocked:
                SoundManager.Instance.Play(SoundTypes.ButtonClick);
                SceneManager.LoadScene(levelToLoad);
                break;
            case LevelStatus.Completed:
                SoundManager.Instance.Play(SoundTypes.ButtonClick);
                SceneManager.LoadScene(levelToLoad);
                break;
        }

         
    }

    private IEnumerator DeactivateLevelLockedScreen()
    {
        yield return new WaitForSeconds(2);
        levelLockedWarning.SetActive(false);
    }
}
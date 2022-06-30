using Enums;
using Managers;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour 
{
    public string levelToLoad;

    public void LoadLevel()
    {   
        LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelToLoad);
        switch (levelStatus) {
            
            case LevelStatus.Locked:
                // SoundManager.Instance.Play(Sounds.ButtonClick);
                Debug.Log("This level is locked!!");
                break;
            case LevelStatus.Unlocked:
                // SoundManager.Instance.Play(Sounds.ButtonClick);
                SceneManager.LoadScene(levelToLoad);
                break;
            case LevelStatus.Completed:
                // SoundManager.Instance.Play(Sounds.ButtonClick);
                SceneManager.LoadScene(levelToLoad);
                break;
        }
    }
}
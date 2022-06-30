using System;
using Enums;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoGenericSingleton<LevelManager>
    {
        public string[] levels;
        private void Start() 
        {
            if (GetLevelStatus(levels[0]) == LevelStatus.Locked)
            {
                SetLevelStatus(levels[0], LevelStatus.Unlocked);
            }
        }
        
        // ToDo: Call this func when level completes.
        public void MarkLevelComplete() 
        {
            Scene currentScene = SceneManager.GetActiveScene();
        
            //  Set level status to completed 
            SetLevelStatus(currentScene.name, LevelStatus.Completed);       

            //  Unlock the next level        
            int currentSceneIndex = Array.FindIndex(levels, level => level == currentScene.name); 
            int nextSceneIndex = currentSceneIndex + 1;
            if(currentSceneIndex < levels.Length) {
                SetLevelStatus(levels[nextSceneIndex], LevelStatus.Unlocked);
            }
        }

        public LevelStatus GetLevelStatus(string level)
        {
            LevelStatus levelStatus = (LevelStatus) PlayerPrefs.GetInt(level, 0);
            return levelStatus;
        }
        public void SetLevelStatus(string level, LevelStatus levelStatus)
        {
            PlayerPrefs.SetInt(level, (int)levelStatus);
            Debug.Log("Setting " + level +" status " + levelStatus);
        }
    }
}



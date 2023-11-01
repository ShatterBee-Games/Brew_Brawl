// zoe - 2023
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace zoe
{
    public class SceneAndGameManager : MonoBehaviour
    {
        
        public void LoadNextSceneWithDelay()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ChangeToFairy()
        {
            PlayerPrefs.SetInt("IsGnome", 0);
            PlayerPrefs.SetInt("IsFairy", 1);
            PlayerPrefs.SetInt("IsFrog", 0);
            PlayerPrefs.SetInt("IsBee", 0);
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }

        public void ChangeToGnome()
        {
            PlayerPrefs.SetInt("IsGnome", 1);
            PlayerPrefs.SetInt("IsFairy", 0);
            PlayerPrefs.SetInt("IsFrog", 0);
            PlayerPrefs.SetInt("IsBee", 0);
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }

        public void ChangeToFrog()
        {
            PlayerPrefs.SetInt("IsGnome", 0);
            PlayerPrefs.SetInt("IsFairy", 0);
            PlayerPrefs.SetInt("IsFrog", 1);
            PlayerPrefs.SetInt("IsBee", 0);
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }

        public void ChangeToBee()
        {
            PlayerPrefs.SetInt("IsGnome", 0);
            PlayerPrefs.SetInt("IsFairy", 0);
            PlayerPrefs.SetInt("IsFrog", 0);
            PlayerPrefs.SetInt("IsBee", 1);
                       int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
            
        }
    }
}

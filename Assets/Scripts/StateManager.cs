using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "StateManager", menuName = "ScriptableObjects/StateManager")]
public class StateManager : ScriptableObject
{
    public ScoreSO scoreOB;

    public void SaveAndReloadScene()
    {
        //Handle score recording
        ScoreSO.RecordScore(scoreOB.score, scoreOB.topScore);

        // Restart level by reloading scene on 'i' press
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
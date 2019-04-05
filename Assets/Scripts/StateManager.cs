using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "StateManager", menuName = "ScriptableObjects/StateManager")]
public class StateManager : ScriptableObject
{
    public ScoreSO scoreOB;

    public string isModeInfinite { get; set; }
    public string isSpacingFar { get; set; }
    public string isBouncy { get; set; }
    public string isLeverSpeedRandom { get; set; }
    public string isControlInverted { get; set; }
    public string isControlRandom { get; set; }
    public string isGravityInverted { get; set; }
    public string isHorizontalMode { get; set; }
    public string isNightMode { get; set; }
    public string isEasyMode { get; set; }

    void Awake()
    {
        SetStateBools();
    }

    public void SetStateBools()
    {
        isModeInfinite = PlayerPrefs.GetString("isModeInfinite", "true");
        isSpacingFar = PlayerPrefs.GetString("isSpacingFar", "false");
        isControlInverted = PlayerPrefs.GetString("isControlInverted", "false");
        isControlRandom = PlayerPrefs.GetString("isControlRandom", "false");
        isLeverSpeedRandom = PlayerPrefs.GetString("isLeverSpeedRandom", "false");
        isBouncy = PlayerPrefs.GetString("isBouncy", "false");
        isGravityInverted = PlayerPrefs.GetString("isGravityInverted", "false");
        isHorizontalMode = PlayerPrefs.GetString("isHorizontalMode", "false");
        isNightMode = PlayerPrefs.GetString("isNightMode", "false");
        isEasyMode = PlayerPrefs.GetString("isEasyMode", "false");
    }

    public void SaveAndReloadScene()
    {
        //Handle score recording
        scoreOB.RecordScore(scoreOB.score, scoreOB.topScore);

        // Restart level by reloading scene on 'i' press
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
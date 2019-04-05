using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSO", menuName = "ScriptableObjects/ScoreSO")]
public class ScoreSO : ScriptableObject
{
    public string score { get; set; }
    public string topScore { get; set; }

    public void InitializeScores()
    {
        score = "0";
        topScore = PlayerPrefs.GetString("topInfiniteScore", "0");
    }

    public void RecordScore(string score, string topScore)
    {
        if (int.Parse(score) > int.Parse(topScore))
        {
            PlayerPrefs.SetString("topInfiniteScore", score);
        }
    }
}

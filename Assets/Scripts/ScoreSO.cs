using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSO", menuName = "ScriptableObjects/ScoreSO")]
public class ScoreSO : ScriptableObject
{
    private string _score;
    private string _topScore;
    public string score { get => _score; set => _score = value; }
    public string topScore { get => _topScore; set => _topScore = value; }

    public static void RecordScore(string score, string topScore)
    {
        if (int.Parse(score) > int.Parse(topScore))
        {
            PlayerPrefs.SetString("topInfiniteScore", score);
        }
    }
}

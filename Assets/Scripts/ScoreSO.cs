using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSO", menuName = "ScriptableObjects/ScoreSO")]
public class ScoreSO : ScriptableObject
{
    //private string score;
    //private string topScore;

    //Can also write as: public string score { get; set; }
    private string _score;
    private string _topScore;
    public string score { get => _score; set => _score = value; }
    public string topScore { get => _topScore; set => _topScore = value; }

    // I guess I'm too used to Java.
    // It's good to know how to do it I suppose.
    //public string GetScore()
    //{
    //    return score;
    //}

    //public void SetScore(string score)
    //{
    //    this.score = score;
    //}

    //public string GetTopScore()
    //{
    //    return topScore;
    //}

    //public void SetTopScore(string topScore)
    //{
    //    this.topScore = topScore;
    //}

    public static void RecordScore(string score, string topScore)
    {
        if (int.Parse(score) > int.Parse(topScore))
        {
            PlayerPrefs.SetString("topInfiniteScore", score);
        }
    }
}

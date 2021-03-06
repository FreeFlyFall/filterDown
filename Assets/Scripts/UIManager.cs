﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ScoreSO scoreOB;

    public TextMeshProUGUI infiniteScoreText;
    public TextMeshProUGUI topInfiniteScoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreOB.InitializeScores();

        // Start tracking ininite game mode score
        StartCoroutine(SetInfiniteScoreText());

        // Set score UI
        topInfiniteScoreText.SetText("Top score: " + scoreOB.topScore);
    }

    IEnumerator SetInfiniteScoreText()
    {
        int score = int.Parse(scoreOB.score);
        int topScore = int.Parse(scoreOB.topScore);
        if (score > 0)
        {
            infiniteScoreText.SetText("Score: " + score);
        }
        else
        {
            infiniteScoreText.SetText("Score: 0");
        }
        if (score > topScore)
        {
            topInfiniteScoreText.SetText("Top score: " + score);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetInfiniteScoreText());
    }
}

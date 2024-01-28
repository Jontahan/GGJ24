using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[Serializable]
public class ScoreVariable
{
    public string scoreName;
    public float scoreValue;
}

[Serializable]
public class ScoreManager
{
    public float baseScore;
    public List<ScoreVariable> scores = new();

    public List<ScoreVariable> scoreModifiers = new();
    public List<ScoreVariable> ScoreModifiers
    {
        get { return scoreModifiers; }
        set { scoreModifiers = value; }
    }

    public float GetFloatScores(int index)
    {
        return scores[index].scoreValue;
    }

    public float GetFloatScoreModifier(int index)
    {
        return scoreModifiers[index].scoreValue;
    }

    public int GetTotalScore()
    {
        float totalScore = 0;
        foreach (var score in scores)
        {
            totalScore += score.scoreValue;
        }

        float totalScoreModifier = 0;
        foreach (var score in scoreModifiers)
        {
            totalScoreModifier += score.scoreValue;
        }

        return (int)Math.Round(baseScore + (totalScore * totalScoreModifier));
    }
}

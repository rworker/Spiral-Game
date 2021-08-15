using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public event Action OnScoreUpdated; //sends signal to update score UI


    private int score;
    public int Score // demonstrate use of property
    {
        get {return score;}
        set 
        {
            if (value < 0)
            {
                score = 0;
            }
            else
            {
                score = value;
            }
        }
    }

    public int baseScore;
    public int finalScore;


    public void AddtoScore(int scoreToAdd)
    {
        score += scoreToAdd;
        OnScoreUpdated();
    }
}

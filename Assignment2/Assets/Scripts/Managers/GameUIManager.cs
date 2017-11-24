﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] Text timerText;
    [SerializeField] float timeRemaining = 0.0f;
    [Header("Score")]
    [SerializeField] Text scoreText;
    [SerializeField] float score = 0.0f;
    [SerializeField] Text scoreMultiplierText;
    [SerializeField] float scoreMultiplier = 1.0f;
    [SerializeField] MultiplierColour[] multiplierColour;


    public void UpdateTimeRemaining(float _timeRemaining)
    {   //Updating the timer on HUD.
        if(_timeRemaining != timeRemaining)
        {   //If the timer has changed, update the text.
            timerText.text = (Mathf.Round(timeRemaining * 10) / 10f).ToString("00.0");
        }

        timeRemaining = _timeRemaining;
    }

    public void UpdateScore(float _score)
    {   //Updating the score on HUD.
        if(_score != score)
        {   //If the score has changed, update the text.
            scoreText.text = (Mathf.Round(score)).ToString();
        }

        score = _score;
    }

    public void UpdateScoreMultiplier(float _scoreMultiplier)
    {   //Updating the score multiplier on HUD.
        if (_scoreMultiplier != scoreMultiplier)
        {   //If the score multiplier has changed, update the text.
            scoreMultiplierText.text = ("x" + scoreMultiplier.ToString());
            for (int i = 0; i < multiplierColour.Length; i++)
            {   //Check through the MultiplierColour for where our current multiplier sits and assign the respecive colour to our text.
                if (scoreMultiplier <= multiplierColour[i].multiplier)
                {   //For some reason, I need to break the colour and reconstruct it like so. Unity won't take = multiplierColour[i].colour as it is.
                    scoreMultiplierText.color = new Color(multiplierColour[i].colour.r, multiplierColour[i].colour.g, multiplierColour[i].colour.b);
                    break;
                }
            }
        }

        scoreMultiplier = _scoreMultiplier;
    }

}

[System.Serializable]
public struct MultiplierColour
{   //Scruct for our multiplier colours, each entry will contain a multiplier and a relative colour.
    public float multiplier;
    public Color colour;
}
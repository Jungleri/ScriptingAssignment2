using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public EGameState gameState;

    [Header("Timer")]
    [SerializeField] Text timerText;
    [SerializeField] float timeRemaining = 0.0f;

    [Header("Score")]
    [SerializeField] Text scoreText;
    [SerializeField] float score = 0.0f;
    [SerializeField] Text scoreMultiplierText;
    [SerializeField] float scoreMultiplier = 1.0f;
    [SerializeField] MultiplierColour[] multiplierColour;

    [Header("Per-Round Score")]
    [SerializeField] Text endRoundScore;
    [SerializeField] Text[] roundScoreOwners;
    bool endRoundScoreActive = false;


    private void Awake()
    {   //When we are initialized, send reference to ourselves, to the GameManager.
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().uiMang = this;
    }


    public void UpdateTimeRemaining(float _timeRemaining)
    {   //Updating the timer on HUD.
        if(_timeRemaining != timeRemaining)
        {   //If the timer has changed, update the text.
            timerText.text = (Mathf.Round(timeRemaining * 10) / 10f).ToString("00.0");
        }

        timeRemaining = _timeRemaining;
    }


    private void Update()
    {
        if (endRoundScoreActive)
        {
            if (gameState == EGameState.Game)
            {   //If we are in Game mode, and the post-round UI elements are still open, close them.
                CloseEndRoundScore();
                endRoundScoreActive = false;
            }
        }
    }


    public void UpdateScore(float _score)
    {   //Updating the score on HUD.
        score = _score;
        scoreText.text = (Mathf.Round(score)).ToString();
    }


    public void UpdateScoreMultiplier(float _scoreMultiplier)
    {   //Updating the score multiplier on HUD.
        scoreMultiplier = _scoreMultiplier;
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


    public void EndRound(int _level, float _score)
    {   //When a round ends, enable the 'Big Score UI' and update the level score.
        endRoundScore.text = _score.ToString();
        roundScoreOwners[_level - 1].text = _score.ToString();
        endRoundScore.gameObject.SetActive(true);
        endRoundScoreActive = true;
    }


    public void CloseEndRoundScore()
    {   //Disable the large round score.
        endRoundScore.gameObject.SetActive(false);
    }


    public void EndGame()
    {   //Simply start the EndGameDisplay coroutine, which shows the players score per-level.
        StartCoroutine(EndGameDisplay());
    }


    IEnumerator EndGameDisplay()
    {   
        for (int i = 0; i < roundScoreOwners.Length; i++)
        {   //In turn, enable each of the round score text elements.
            yield return new WaitForSeconds(0.5f);
            roundScoreOwners[i].gameObject.SetActive(true);
        }
    }
}


[System.Serializable]
public struct MultiplierColour
{   //Scruct for our multiplier colours, each entry will contain a multiplier and a relative colour.
    public float multiplier;
    public Color colour;
}
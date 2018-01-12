using System.Collections;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public EGameState gameState;
    public float score;
    public float scoreMultiplier = 1.0f;
    float resetTimer = 0f;
    float[] roundScores;


    private void Update()
    {
        if ((gameState == EGameState.Game) && (scoreMultiplier != 1.0f))
        {   //If there is a multiplier, start counting up.
            resetTimer += Time.deltaTime;
        }

        if(resetTimer >= 8.0f)
        {   //If the counter hits 8+, reset the multiplier.
            ResetMultiplier();
        }
    }


    public void EndRound(int _round)
    {
        //roundScores[_round - 1] = score;
        ResetMultiplier();
    }


    public void IncreaseScore(float _scoreToAdd)
    {   //When we want to add score, increase it along side the multiplier.
        score += (_scoreToAdd * scoreMultiplier);
        IncreaseMultiplier();
    }

    
    void IncreaseMultiplier()
    {   //Simply ass 0.1 to the multiplier.
        scoreMultiplier += 0.1f;
    }


    void ResetMultiplier()
    {   //Set the multiplier back to 0;
        scoreMultiplier = 1.0f;
        resetTimer = 0.0f;
    }
}
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
        {
            resetTimer += Time.deltaTime;
        }

        if(resetTimer >= 10.0f)
        {
            ResetMultiplier();
        }
    }


    public void EndRound(int _round)
    {
        //roundScores[_round - 1] = score;
        ResetMultiplier();
    }


    public void IncreaseScore(float _scoreToAdd)
    {
        score += (_scoreToAdd * scoreMultiplier);
        IncreaseMultiplier();
    }

    
    void IncreaseMultiplier()
    {
        scoreMultiplier += 0.1f;
    }


    void ResetMultiplier()
    {
        scoreMultiplier = 1.0f;
        resetTimer = 0.0f;
    }
}
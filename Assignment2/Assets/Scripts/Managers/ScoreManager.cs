using System.Collections;
using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    public EGameState gameState;
    public float score;
    public float scoreMultiplier = 1.0f;
    bool multiplierUpdated = false;


    public void IncreaseScore(float _scoreToAdd)
    {
        score += (_scoreToAdd * scoreMultiplier);
        IncreaseMultiplier();
    }

    
    void IncreaseMultiplier()
    {
        scoreMultiplier += 0.1f;
        StartCoroutine(TimeoutMultiplierReset());
        multiplierUpdated = false;
    }


    void ResetMultiplier()
    {
        scoreMultiplier = 1.0f;
    }


    IEnumerator TimeoutMultiplierReset()
    {
        multiplierUpdated = true;
        while (multiplierUpdated)
        {
            yield return new WaitForSeconds(5);
            ResetMultiplier();
        }
        yield break;
    }
}
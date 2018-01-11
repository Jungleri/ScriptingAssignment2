using UnityEngine;
using System.Diagnostics;


public class TimerManager : MonoBehaviour
{
    public EGameState gameState;

    [Header("ManagerScript")]
    [SerializeField] GameManager gameMang;

    [Header("Timer Values")]
    [SerializeField] float preTimer = 5.0f;
    [SerializeField] float roundTimer = 60.0f;
   public float targetTime;
    Stopwatch stopwatch = new Stopwatch();

    [Header("Public Variables")]
    public float timeRemaining;
    public bool roundActive = false;


    void Update()
    {   //If there is still time on the clock, update it.
        if (targetTime != 0.0f)
        {
            CheckRemainingTime((stopwatch.ElapsedMilliseconds/ 1000f) , targetTime);
        }
    }


    public void PreRoundCountdown()
    {   //Start a timer for the preround.
        targetTime = preTimer;
        stopwatch.Reset();
        stopwatch.Start();
        roundActive = false;
    }


    public void RoundCountdown()
    {   //Start a timer for the level.
        targetTime = roundTimer;
        stopwatch.Reset();
        stopwatch.Start();
        roundActive = true;
    }


    private void CheckRemainingTime(float _elapsedTime, float _targetTime)
    {   //Update the remaining time var.
        timeRemaining = _targetTime - _elapsedTime;
        if (timeRemaining <= 0.0f)
        {   //If we have 0s or less on the clock, stop the stopwatch and soft reset the timer to 0s.
            stopwatch.Stop();
            gameMang.TimerComplete();
            timeRemaining = 0.0f;
        }
    }
}
using UnityEngine;
using System.Diagnostics;


public class TimerManager : MonoBehaviour
{

    [SerializeField] float preTimer = 5.0f;
    [SerializeField] float roundTimer = 60.0f;
    float targetTime;
    Stopwatch stopwatch = new Stopwatch();


    public float timeRemaining;
    public bool roundActive = false;

    void Update()
    {
        if (targetTime != 0.0f)
            CheckRemainingTime(stopwatch.ElapsedMilliseconds, targetTime);
    }


    public void PreRoundCountdown()
    {
        stopwatch.Start();
        targetTime = preTimer;
        roundActive = false;
    }


    public void RoundCountdown()
    {
        stopwatch.Start();
        targetTime = roundTimer;
        roundActive = true;
    }


    private void CheckRemainingTime(float _elapsedTime, float _targetTime)
    {
        {
            timeRemaining = _targetTime - _elapsedTime;
            if (timeRemaining <= 0.0f)
            {
                stopwatch.Stop();
            }
        }
    }
}

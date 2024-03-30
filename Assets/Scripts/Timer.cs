using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    public event UnityAction OnTimerDone;

    private float _startTime;
    private float _duration;
    private float _targetTime;

    private bool isActive;
    public Timer(float duration)
    {
        _duration = duration;
    }

    public void StartTimer()
    {
        _startTime = Time.time;
        _targetTime = _startTime + _duration;
        isActive = true;
    }

    public void StopTimer()
    {
        isActive = false;
    }

    public void Tick()
    {
        if (!isActive)
        {
            return;
        }

        if (Time.time >= _targetTime)
        {
            StopTimer();
            OnTimerDone?.Invoke();      
        }
    }


}

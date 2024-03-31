using System;
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
    private float _currentTime;

    private bool isActive;
    public Timer(float duration)
    {
        _duration = duration;
    }

    public float GetTime => _currentTime;

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

        _currentTime = _targetTime - Time.time;
    }

    public string FloatToString(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return string.Format("{0:00}:{1:00}", (int)timeSpan.TotalMinutes, timeSpan.Seconds);
    }

    public float StringToFloat(string time)
    {
        string[] parts = time.Split(':');

        float minutes = int.Parse(parts[0]); // Int to float, hmmmmm.... :)
        float seconds = int.Parse(parts[1]);

        return minutes * 60 + seconds;
    }
}

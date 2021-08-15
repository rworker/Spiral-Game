using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event EventHandler OnOutOfTime;

    private bool timerActive = false;
    //private bool outOfTime = false;
    public float currentTime;
    public int startMinutes;
    public string currentTimeText;

    private bool alreadyTriggered = false;

    private void Start() 
    {
        currentTime = startMinutes * 60; // sets total seconds left at start of the game based on minutes
        StartTimer();
    }

    private void Update() 
    {
        if (timerActive)
        {
            currentTime = currentTime -= Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        if(time.Seconds < 10)
        {
            currentTimeText = time.Minutes.ToString() + ":0" + time.Seconds.ToString(); //string to be displayed in timer UI
        }
        else
        {
            currentTimeText = time.Minutes.ToString() + ":" + time.Seconds.ToString(); //string to be displayed in timer UI
        }

        if (currentTime <= 0f && !alreadyTriggered)
        {
            
            if (OnOutOfTime != null) OnOutOfTime(this, EventArgs.Empty); //if OnOutOfTime has subscribers, trigger
            alreadyTriggered = true;
        }
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }

}

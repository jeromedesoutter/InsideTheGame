using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TimeStruct
{
    public int minutes;
    public int seconds;

    public void SetTime(int min,int sec)
    {
        minutes = min + (int)(sec / 60);
        seconds = sec % 60;
    }

    public void SetTime(int sec)
    {
        minutes = (int)(sec / 60);
        seconds = sec % 60;
    }
    public int GetSeconds()
    {
        return minutes * 60 + seconds;
    }
};

public class Timer : MonoBehaviour
{
    public Text timerText;
    private double currentTime = 0;
    private bool timerActive;

    public TimeStruct time;
    public bool timeOver = false;

    void Start()
    {
        ResetTimer();
    }

    void FixedUpdate()
    {
        if (timerActive)
        {
            if (currentTime > time.GetSeconds()) timeOver = true;
            currentTime += Time.deltaTime;
            UpdateOnInterface();
        }
        else if (timeOver)
        {
            timeOver = false;
        }
    } 


    public void ResetTimer(int seconds=10, int minutes=0) {
        currentTime = 0;
        timerActive = true;
        timeOver = false;
        time = new TimeStruct();
        time.SetTime(minutes, seconds);
    }
    void UpdateOnInterface()
    {
        TimeStruct remainingTime = new TimeStruct();
        remainingTime.SetTime(Mathf.Max(0, time.GetSeconds() - (int)currentTime));
        timerText.text = "Temps restant     " + remainingTime.minutes.ToString("00") + ":" + remainingTime.seconds.ToString("00");
    }

    public void StopTimer()
    {
        timerActive = false;
    }
    public void ContinueTimer()
    {
        timerActive = true;
    }

    public bool IsActive()
    {
        return timerActive;
    }
}

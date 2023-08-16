using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

[Serializable]
public enum TimeZone
{
    Past,
    Present,
    Future
}

public class TimeZoneManager : MonoBehaviour
{
    private TimeZone nowTimeZone;
    public TimeZone NowTimeZone => nowTimeZone;
    [SerializeField] private TMP_Text timeZoneText; //temp
    private Dictionary<TimeZone, bool> canTimeMoveDict = new() //Set by ChangeTimeMoveBool() method
    {
        {TimeZone.Past, false},
        {TimeZone.Present, true},
        {TimeZone.Future, false}
    };

    [SerializeField] private GameEvent timeZoneChangeEvent;
    private void ChangeTime(TimeZone timeZone)
    {
        if (!canTimeMoveDict[timeZone]) 
        {
            return;
        }
        nowTimeZone = timeZone;
        GameManager.Instance.TimeManager.NowTimeLockedObject?.GetTimeUnLocked();
        SceneInitialize();
        switch(timeZone)
        {
            case TimeZone.Past:
                timeZoneText.text = "Past";
                break;
            case TimeZone.Present:
                timeZoneText.text = "Present";
                break;
            case TimeZone.Future:
                timeZoneText.text = "Future";
                break;
            default:
                new ArgumentException("Abnormal TimeZone");
                break;
        }

        timeZoneChangeEvent.Raise(this, null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeTime(TimeZone.Past);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeTime(TimeZone.Present);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeTime(TimeZone.Future);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeTimeMoveBool(TimeZone.Past);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeTimeMoveBool(TimeZone.Present);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeTimeMoveBool(TimeZone.Future);
        }
    }

    private void SceneInitialize()
    {
    }

    public void ChangeTimeMoveBool(TimeZone timeZone, bool canMove)
    {
        canTimeMoveDict[timeZone] = canMove;
    }
    public void ChangeTimeMoveBool(TimeZone timeZone)
    {
        canTimeMoveDict[timeZone] = !canTimeMoveDict[timeZone];
    }
}

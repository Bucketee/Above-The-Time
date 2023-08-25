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

    [SerializeField] private TimeZone nowTimeZone = TimeZone.Present;
    public TimeZone NowTimeZone => nowTimeZone;
    [SerializeField] private TMP_Text timeZoneText; //temp
    private Dictionary<TimeZone, bool> canTimeMoveDict = new() //Set by ChangeTimeMoveBool() method
    {
        {TimeZone.Past, true},
        {TimeZone.Present, true},
        {TimeZone.Future, true}
    };

    [SerializeField] private int nowYear;
    private Dictionary<int, TimeZone> yearTimeZoneDict = new()
    {
        {1950, TimeZone.Past},
        {1955, TimeZone.Present},
        {2000, TimeZone.Future}
    };

    [SerializeField] private GameEvent timeZoneChangeEvent;

    [SerializeField] private int hourHandPlus, minuteHandPlus;

    private SoundManager soundManager;
    private void Start()
    {
        soundManager = GameManager.Instance.SoundManager;
        nowYear = 2000;
    }

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
        if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance.GameStateManager.NowGameState == GameState.Playing)
        {
            GameManager.Instance.GameStateManager.ChangeGameState(GameState.TimeChanging);
            StartCoroutine(TimeClockOperate());
        }

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

    private IEnumerator TimeClockOperate()
    {
        hourHandPlus = 0;
        minuteHandPlus = 0;
        yield return new WaitForEndOfFrame();
        while (!Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKeyDown(KeyCode.I)) { hourHandPlus--; soundManager.ClockTickSound(); }
            else if (Input.GetKeyDown(KeyCode.O)) { hourHandPlus++; soundManager.ClockTickSound(); }
            else if (Input.GetKeyDown(KeyCode.K)) { minuteHandPlus--; soundManager.ClockTickSound(); }
            else if (Input.GetKeyDown(KeyCode.L)) { minuteHandPlus++; soundManager.ClockTickSound(); }
            yield return null;
        }
        int finalClockHandInput = hourHandPlus * 12 + minuteHandPlus;
        if (yearTimeZoneDict.ContainsKey(nowYear + finalClockHandInput))
        {
            if (canTimeMoveDict[yearTimeZoneDict[nowYear + finalClockHandInput]])
            {
                nowYear += finalClockHandInput;
                ChangeTime(yearTimeZoneDict[nowYear]);
                Debug.Log("Time Travel Successed!");
            }
            else { Debug.Log("Time Travel Failed!"); }
        }
        else { Debug.Log("Time Travel Failed!"); }
        GameManager.Instance.GameStateManager.RedoGameState();
        yield return new WaitForEndOfFrame();
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

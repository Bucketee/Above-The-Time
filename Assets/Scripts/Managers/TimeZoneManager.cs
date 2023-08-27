using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private TimeZone nowTimeZone;
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
    private Dictionary<TimeZone, int> timeZoneYearDict = new()
    {
        {TimeZone.Past, 1950},
        {TimeZone.Present, 1955},
        {TimeZone.Future, 2000}
    };

    [SerializeField] private GameEvent timeZoneChangeEvent;

    [Header("Clock Operate")]
    [SerializeField] private GameObject ClockFrame;
    [SerializeField] private GameObject Hourhand;
    [SerializeField] private GameObject Minutehand;
    [SerializeField] private GameObject[] descriptions;
    [SerializeField] private TMP_Text nowYearText;
    [SerializeField] public GameObject Operatinghand = null;
    [SerializeField] public int hourHandPlus, minuteHandPlus;

    private void Start()
    {
        nowTimeZone = DataManager.Instance.NowTimeZone;
        nowYear = timeZoneYearDict[nowTimeZone];
        foreach(var desc in descriptions) { desc.SetActive(false); }

        ChangeTime(nowTimeZone, false);
    }

    private void ChangeTime(TimeZone timeZone, bool fadein)
    {
        if (!canTimeMoveDict[timeZone]) 
        {
            return;
        }
        if(fadein) GameManager.Instance.TimeZoneLoading.LoadingFadeIn();
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
        DataManager.Instance.SetTimeZone(nowTimeZone);
        UpdateCanMove();
        

        timeZoneChangeEvent.Raise(this, null);
    }

    public void UpdateCanMove()
    {
        bool[] CanMoves = DataManager.Instance.TimeZoneCanMoves;
        for(int i=0; i<CanMoves.Length; i++)
        {
            ChangeTimeMoveBool((TimeZone) i, CanMoves[i]);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Operatinghand = null;
        }

        if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance.GameStateManager.NowGameState == GameState.Playing)
        {
            GameManager.Instance.GameStateManager.ChangeGameState(GameState.TimeChanging);
            StartCoroutine(TimeClockOperate());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeTime(TimeZone.Past, true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeTime(TimeZone.Present, true);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeTime(TimeZone.Future, true);
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

        if (GameManager.Instance.GameStateManager.NowGameState == GameState.TimeChanging && ClockFrame.activeSelf == false)
        {
            ClockFrame.SetActive(true);
            Hourhand.SetActive(true);
            Minutehand.SetActive(true);
            Hourhand.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Minutehand.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (GameManager.Instance.GameStateManager.NowGameState != GameState.TimeChanging && ClockFrame.activeSelf == true)
        {
            ClockFrame.SetActive(false);
            Hourhand.SetActive(false);
            Minutehand.SetActive(false);
        }

        if (GameManager.Instance.GameStateManager.NowGameState == GameState.TimeChanging)
        {
            Hourhand.transform.rotation = Quaternion.Euler(0f, 0f, -30f * hourHandPlus);
            Minutehand.transform.rotation = Quaternion.Euler(0f, 0f, -30f * minuteHandPlus);
            foreach(var desc in descriptions) { desc.SetActive(true); }
            nowYearText.text = "You are in " + nowYear.ToString();
        }
        else { foreach (var desc in descriptions) { desc.SetActive(false); } }

        ClockFrame.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 10f);
        Hourhand.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 10f);
        Minutehand.transform.position = Camera.main.transform.position + new Vector3(0f, 0f, 10f);
    }

    private IEnumerator TimeClockOperate()
    {
        hourHandPlus = 0;
        minuteHandPlus = 0;
        yield return new WaitForEndOfFrame();
        while (!Input.GetKeyDown(KeyCode.R))
        {
            yield return null;
        }
        int finalClockHandInput = hourHandPlus * 12 + minuteHandPlus;
        if (yearTimeZoneDict.ContainsKey(nowYear + finalClockHandInput))
        {
            if (canTimeMoveDict[yearTimeZoneDict[nowYear + finalClockHandInput]] && finalClockHandInput != 0)
            {
                nowYear += finalClockHandInput;
                ChangeTime(yearTimeZoneDict[nowYear], true);
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

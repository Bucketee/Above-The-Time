using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneLever : MonoBehaviour, ITimeZoneInterface
{
    public Dictionary<TimeZone, Lever.LeverState> leverStateDict = new();
    [SerializeField] private TimeZoneObjectInfo timeZoneObjectInfo;
    private Lever lever;
    private TimeLockLever timeLockLever;
    private TimeZoneManager timeZoneManager;

    public void Setting()
    {
        TimeZone timeZone = GameManager.Instance.TimeZoneManager.NowTimeZone;
        lever.leverState = leverStateDict[timeZone];
        timeLockLever.leverStates = new();
        timeLockLever.leverStatesTemp = new();
        lever.LeverInitialize();
    }

    private void Awake()
    {
        lever = GetComponent<Lever>();
        timeLockLever = GetComponent<TimeLockLever>();
    }

    private void Start()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;

        foreach (TimeZoneV3Active t in timeZoneObjectInfo.timeZoneV3s)
        {
            if (!leverStateDict.ContainsKey(t.timeZone))
            {
                leverStateDict.Add(t.timeZone, t.leverState);
            }
        }
        Setting();
    }

    public void Record(TimeZone timeZone)
    {
        string text = gameObject.name + " ";
        foreach (TimeZone t in Enum.GetValues(typeof(TimeZone)))
        {
            if ((int)t >= (int)timeZone)
            {
                leverStateDict[t] = lever.leverState;
            }
            text += t + ": " + leverStateDict[t] + "/ ";
        }
        Debug.Log(text);
    }

    [Serializable]
    public class TimeZoneObjectInfo
    {
        public List<TimeZoneV3Active> timeZoneV3s;
    }

    [Serializable]
    public class TimeZoneV3Active
    {
        public TimeZone timeZone;
        public Lever.LeverState leverState;
    }
}

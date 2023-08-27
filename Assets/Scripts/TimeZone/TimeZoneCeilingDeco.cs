using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneCeilingDeco : MonoBehaviour, ITimeZoneInterface
{
    public Dictionary<TimeZone, bool> isBrokenDict = new();
    [SerializeField] private TimeZoneObjectInfo timeZoneObjectInfo;
    private CeilingDeco ceilingDeco;
    private TimeZoneManager timeZoneManager;

    public void Setting()
    {
        TimeZone timeZone = GameManager.Instance.TimeZoneManager.NowTimeZone;
        ceilingDeco.InitializeWall();
        ceilingDeco.broken = isBrokenDict[timeZone];
        if (ceilingDeco.broken)
        {
            ceilingDeco.BreakWall(true, ceilingDeco.explosionPower);
        }
    }

    private void Awake()
    {
        foreach (TimeZoneV3Active t in timeZoneObjectInfo.timeZoneV3s)
        {
            if (!isBrokenDict.ContainsKey(t.timeZone))
            {
                isBrokenDict.Add(t.timeZone, t.isBroken);
            }
        }
        ceilingDeco = GetComponent<CeilingDeco>();
    }

    private void Start()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;
    }

    public void Record(TimeZone timeZone)
    {
        string text = gameObject.name + " ";
        foreach (TimeZone t in Enum.GetValues(typeof(TimeZone)))
        {
            if ((int)t >= (int)timeZone)
            {
                isBrokenDict[t] = ceilingDeco.broken;
            }
            text += t + ": " + isBrokenDict[t] + "/ ";
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
        public bool isBroken;
    }
}

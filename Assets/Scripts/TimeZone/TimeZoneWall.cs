using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneWall : MonoBehaviour, ITimeZoneInterface
{
    public Dictionary<TimeZone, bool> isBrokenDict = new();
    [SerializeField] private TimeZoneObjectInfo timeZoneObjectInfo;
    private Wall wall;
    private TimeZoneManager timeZoneManager;

    public void Setting()
    {
        TimeZone timeZone = GameManager.Instance.TimeZoneManager.NowTimeZone;
        wall.InitializeWall();
        wall.broken = isBrokenDict[timeZone];
        if (wall.broken)
        {
            wall.BreakWall(true, wall.explosionPower);
        }
    }

    private void Awake()
    {
        wall = GetComponent<Wall>();

        foreach (TimeZoneV3Active t in timeZoneObjectInfo.timeZoneV3s)
        {
            if (!isBrokenDict.ContainsKey(t.timeZone))
            {
                isBrokenDict.Add(t.timeZone, t.isBroken);
            }
        }
    }

    private void Start()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;
    }

    public void Record(TimeZone timeZone)
    {
        //string text = gameObject.name + " ";
        foreach (TimeZone t in Enum.GetValues(typeof(TimeZone)))
        {
            if ((int)t >= (int)timeZone)
            {
                isBrokenDict[t] = wall.broken;
            }
            //text += t + ": " + isBrokenDict[t] + "/ ";
        }
        //Debug.Log(text);
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

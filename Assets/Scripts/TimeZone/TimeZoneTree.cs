using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneTree : MonoBehaviour, ITimeZoneInterface
{
    public Dictionary<TimeZone, TimeLockTree.TreeState> treeStateDict = new();
    [SerializeField] private TimeZoneObjectInfo timeZoneObjectInfo;
    private TimeLockTree timeLockTree;
    private TimeZoneManager timeZoneManager;

    public void Setting()
    {
        TimeZone timeZone = GameManager.Instance.TimeZoneManager.NowTimeZone;
        timeLockTree.state = treeStateDict[timeZone];
        timeLockTree.ChangeShape();
    }

    private void Awake()
    {
        timeLockTree = GetComponent<TimeLockTree>();
        foreach (TimeZoneV3Active t in timeZoneObjectInfo.timeZoneV3s)
        {
            if (!treeStateDict.ContainsKey(t.timeZone))
            {
                treeStateDict.Add(t.timeZone, t.treeState);
            }
        }
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
                treeStateDict[t] = timeLockTree.state;
            }
            text += t + ": " + treeStateDict[t] + "/ ";
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
        public TimeLockTree.TreeState treeState;
    }
}

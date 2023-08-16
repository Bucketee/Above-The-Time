using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private TimeLockObject nowTimeLockedObject = null;
    public TimeLockObject NowTimeLockedObject => nowTimeLockedObject;

    public void SetNowTimeLockedObject(TimeLockObject timeLockObject)
    {
        nowTimeLockedObject?.GetTimeUnLocked();
        nowTimeLockedObject = timeLockObject;
    }

    public void UnSetNowTimeLockedObject()
    {
        nowTimeLockedObject = null;
    }
}

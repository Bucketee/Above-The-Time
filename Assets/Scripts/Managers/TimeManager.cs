using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private TimeLockObject nowTimeLockedObject = null;
    public TimeLockObject NowTimeLockedObject => nowTimeLockedObject;

    public void SetNowTimeLockedObject(TimeLockObject timeLockObject)
    {
        if (nowTimeLockedObject != null) // timeComponent.gameObject == nowTimeLockedObject인 경우 이 함수가 호출되지 않아서 고려 X
        {
            nowTimeLockedObject.GetTimeUnLocked();
        }
        nowTimeLockedObject = timeLockObject;
    }

    public void UnSetNowTimeLockedObject()
    {
        nowTimeLockedObject = null;
    }
}

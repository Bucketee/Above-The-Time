using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockAxe : TimeLockObject
{
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        Vector3 pos = transform.position;
        Vector3 target = pos + new Vector3(50f, 0, 0);
        while (pos.x <= target.x)
        {
            positions.AddFirst(new PositionInTime(pos, Quaternion.identity, Vector2.zero, 0));
            pos = pos + new Vector3(0.3f, 0, 0);
        }
    }

    public override void GetTimeUnLocked()
    {
        maxDuration = 0f;
        currentDuration = 0f;
        GameManager.Instance.TimeManager.UnSetNowTimeLockedObject();
        timeLocked = false;
        timeLockCoolDownTime = timeLockCoolDown;
        positionsTemp.Clear();
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        ApplyMovement();
        if (nowTimeLockCoroutine != null)
        {
            StopCoroutine(nowTimeLockCoroutine);
            nowTimeLockCoroutine = null;
        }
        SetSortingLayer("Default");
    }

    protected override void Record()
    {
        if (timeLocked || (positions.Count > 0 && (positions.Last.Value.position - rigidbody2D.position).magnitude <= 0.01f)) return;
        if (positions.Count >= maxRecordSize)
        {
            positions.RemoveFirst();
        }
        positions.AddLast(new PositionInTime(transform.position, transform.rotation, rigidbody2D.velocity, rigidbody2D.angularVelocity));
    }
}

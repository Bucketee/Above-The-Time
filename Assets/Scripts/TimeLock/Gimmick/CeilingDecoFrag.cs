using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDecoFrag : WallFragment
{
    public FixedJoint2D joint2D;
    public LayerMask initLayer;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        wallObject = GetComponentInParent<Wall>();
        firstPos = rigidbody2D.position;
        firstRot = rigidbody2D.rotation;
        joint2D = GetComponent<FixedJoint2D>();
        initLayer = gameObject.layer;
    }

    protected override void Record()
    {
        if (wallObject.timeLocked || (positions.Count > 0 && (positions.Last.Value.position - rigidbody2D.position).magnitude <= 0.01f)) return;
        positions.AddLast(new PositionInTime(transform.position, transform.rotation, rigidbody2D.velocity, rigidbody2D.angularVelocity));
    }

    public override void GetTimeUnLocked()
    {
        if (wallObject.timeLocked)
        {
            wallObject.TimeUnLocked();
            return;
        }
        GameManager.Instance.TimeManager.UnSetNowTimeLockedObject();
        timeLocked = false;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        if (positions.Count > 0)
        {
            speed = positions.Last.Value.speed;
            angular = positions.Last.Value.angular;
        }
        ApplyMovement();
        if (nowTimeLockCoroutine != null)
        {
            StopCoroutine(nowTimeLockCoroutine);
            nowTimeLockCoroutine = null;
        }
        SetSortingLayer("Default");
    }

    protected override IEnumerator GoToFirstPosCo()
    {
        rewinding = true;
        rigidbody2D.velocity = new Vector2(0f, 0f);
        rigidbody2D.angularVelocity = 0f;
        while (positions.Count > 0)
        {
            yield return new WaitForSeconds((positions.Last.Value.position - rigidbody2D.position).magnitude / (50 * rewindingSpeed * Time.deltaTime));
            Rewind();
        }
        transform.position = firstPos;
        transform.rotation = Quaternion.Euler(0f, 0f, firstRot);
        speed = new Vector2(0f, 0f);
        angular = 0f;
        positions = new LinkedList<PositionInTime>();
        rewinding = false;
        wallObject.AddRewindCount();
    }

    public override void GoToFirstPosNow()
    {
        rigidbody2D.velocity = new Vector2(0f, 0f);
        rigidbody2D.angularVelocity = 0f;
        transform.position = firstPos;
        transform.rotation = Quaternion.Euler(0f, 0f, firstRot);
        speed = new Vector2(0f, 0f);
        angular = 0f;
        positions = new LinkedList<PositionInTime>();
        wallObject.AddRewindCount();
    }
}

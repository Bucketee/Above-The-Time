using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFragment : TimeLockObject
{
    [Header("State")]
    protected Vector2 firstPos;
    protected float firstRot;
    protected bool rewinding = false;
    protected float rewindingSpeed;
    protected float initialRot = 0;

    [Header("Vibrate")]
    protected float vibration = 1.5f;
    protected Coroutine vibrateCo = null;

    protected Wall wallObject;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        wallObject = GetComponentInParent<Wall>();
        firstPos = rigidbody2D.position;
        firstRot = rigidbody2D.rotation;
    }

    private void Update()
    {
        if (timeLocked || rewinding) return;
        Record();
    }

    public override void HandleTimeLock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (rewinding || wallObject.rewinding) return;
            if (timeLocked)
            {
                wallObject.TimeUnLocked();
            }
            else
            {
                GameManager.Instance.TimeManager.SetNowTimeLockedObject(this);
                wallObject.TimeLocked();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<MouseCursor>(out MouseCursor mouse)) return;
        wallObject.SetFragSortingLayer("timelockable");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<MouseCursor>(out MouseCursor mouse)) return;
        wallObject.SetFragSortingLayer("default");
    }

    protected override void Record()
    {
        //if (wallObject.timeLocked || gameObject.layer == LayerMask.NameToLayer("Wall") || rigidbody2D.velocity == new Vector2(0, 0)) return; //337
        if (wallObject.timeLocked || gameObject.layer == LayerMask.NameToLayer("Wall") || (positions.Count > 0 && (positions.Last.Value.position - rigidbody2D.position).magnitude <= 0.01f)) return; //73
        //return nothing => 489
        positions.AddLast(new PositionInTime(transform.position, transform.rotation, rigidbody2D.velocity, rigidbody2D.angularVelocity));
        //Debug.Log("recording");
    }

    public void ExplosionEffect(Vector2 loc, float power)
    {
        //Debug.Log("exploding");
        GetComponent<Rigidbody2D>().AddForceAtPosition((GetComponent<Rigidbody2D>().position - loc) * power / (GetComponent<Rigidbody2D>().position - loc).magnitude, loc, ForceMode2D.Impulse);
    }

    public void Vibrating()
    {
        //Debug.Log("vibrate");
        if (vibrateCo != null) return;
        initialRot = rigidbody2D.rotation;
        vibrateCo = StartCoroutine(VibratingCo());
    }

    public void StopVibrating()
    {
        //Debug.Log("vibrate");
        if (vibrateCo != null) StopCoroutine(vibrateCo);
        vibrateCo = null;
        rigidbody2D.rotation = initialRot;
    }

    private IEnumerator VibratingCo()
    {
        while (timeLocked && !rewinding)
        {
            //Debug.Log("asd");
            rigidbody2D.rotation = initialRot + Mathf.PingPong(Time.time * wallObject.count * 10, 2 * vibration) - vibration;
            yield return null;
        }
        if (!timeLocked)
        {
            StopVibrating();
        }
    }

    public void GoToFirstPos()
    {
        StartCoroutine(GoToFirstPosCo());
        //Debug.Log("go to first");
    }

    protected virtual IEnumerator GoToFirstPosCo()
    {
        rewinding = true;
        rigidbody2D.velocity = new Vector2(0f, 0f);
        rigidbody2D.angularVelocity = 0f;
        while (positions.Count > 0)
        {
            yield return new WaitForSeconds((positions.Last.Value.position - rigidbody2D.position).magnitude / (50 * rewindingSpeed * Time.deltaTime));
            Rewind();
            //Debug.Log("rewinding");
        }
        transform.position = firstPos;
        transform.rotation = Quaternion.Euler(0f, 0f, firstRot);
        speed = new Vector2(0f, 0f);
        angular = 0f;
        gameObject.layer = LayerMask.NameToLayer("Wall");
        positions = new LinkedList<PositionInTime>();
        rewinding = false;
        wallObject.AddRewindCount();
    }

    public override void GetTimeLocked()
    {
        timeLocked = true;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.velocity = new Vector2(0f, 0f);
        rigidbody2D.angularVelocity = 0f;
    }

    public override void GetTimeUnLocked()
    {
        //Debug.Log("UnLocked!!");
        if (wallObject.timeLocked)
        {
            wallObject.TimeUnLocked();
            return;
        }
        GameManager.Instance.TimeManager.UnSetNowTimeLockedObject();
        timeLocked = false;
        if (gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("WallFragment"))
        {
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        if (positions.Count > 0)
        {
            speed = positions.Last.Value.speed;
            angular = positions.Last.Value.angular;
        }
        ApplyMovement();
    }

    public void SetRewindSpeed(float speed)
    {
        rewindingSpeed = speed;
    }

    public void SetTimeLock(bool state)
    {
        timeLocked = state;
    }
}
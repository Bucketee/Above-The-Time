using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TimeLockObject : MonoBehaviour
{
    protected new Rigidbody2D rigidbody2D;

    [Header("Time Lock")]
    [SerializeField] protected bool timeLocked = false;
    public float timeLockCoolDown;
    public bool TimeLocked => timeLocked;
    [SerializeField] protected float duration = 10f;
    protected LinkedList<PositionInTime> positions = new ();
    protected LinkedList<PositionInTime> positionsTemp = new (); //for future
    [SerializeField] protected int maxRecordSize = 20;
    protected float timeLockCoolDownTime = 0;
    protected bool canTimeLock = true;
    public int Count => positions.Count;

    protected Vector2 speed;
    protected float angular;
    protected Coroutine nowTimeLockCoroutine = null;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void HandleTimeLock()
    {
        if (timeLocked)
        {
            if (nowTimeLockCoroutine != null)
            {
                StopCoroutine(nowTimeLockCoroutine);
                nowTimeLockCoroutine = null;
            }
            GetTimeUnLocked();
        }
        else
        {
            GetTimeLocked();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!timeLocked && !(collision.gameObject.layer == LayerMask.NameToLayer("Mouse"))) return;
        SetSortingLayer("Timelockable");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!timeLocked && !(collision.gameObject.layer == LayerMask.NameToLayer("Mouse"))) return;
        SetSortingLayer("Default");
    }

    public void SetSortingLayer(string layer)
    {
        GetComponent<SpriteRenderer>().sortingLayerName = layer;
    }

    public void ChangeSortingLayer()
    {
        if (GetComponent<SpriteRenderer>().sortingLayerName.Equals("Default"))
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Timelockable";
        }
        else
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }
    }

    private void Update()
    {
        if (timeLocked)
        {
            float timeAmount = Input.GetAxis("Mouse ScrollWheel");
            if (timeAmount < 0)
            {
                for (int i = 0; i < (0.01f / Time.deltaTime); i++) Rewind();
            }
            else if (timeAmount > 0)
            {
                for (int i = 0; i < (0.01f / Time.deltaTime); i++) UnRewind();
            }
            return;
        }
        else 
        {
            if (timeLockCoolDownTime > 0) { timeLockCoolDownTime -= 1f * Time.deltaTime; canTimeLock = false; }
            else { canTimeLock = true; }
        }
        Record();
    }

    protected virtual IEnumerator TimeLockDuration(float duration)
    {
        float time = Time.time + duration;
        while (Time.time < time)
        {
            yield return new WaitForSeconds((time - Time.time) / 10);
            ChangeSortingLayer();
        }
        GetTimeUnLocked();
    }

    protected virtual void Record()
    {
        if (timeLocked || (positions.Count > 0 && (positions.Last.Value.position - rigidbody2D.position).magnitude <= 0.01f)) return;
        if (positions.Count >= maxRecordSize)
        {
            positions.RemoveFirst();
        }
        positions.AddLast(new PositionInTime(transform.position, transform.rotation, rigidbody2D.velocity, rigidbody2D.angularVelocity));
    }

    public virtual void GetTimeLocked()
    {
        GameManager.Instance.TimeManager.SetNowTimeLockedObject(this);
        timeLocked = true;
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.angularVelocity = 0f;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        nowTimeLockCoroutine = StartCoroutine(TimeLockDuration(duration));
    }

    public virtual void GetTimeUnLocked()
    {
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

    protected virtual void Rewind()
    {
        if (positions.Count > 0)
        {
            PositionInTime positionInTime = positions.Last.Value;
            positions.RemoveLast();
            transform.SetPositionAndRotation(positionInTime.position, positionInTime.rotation);
            speed = positionInTime.speed;
            angular = positionInTime.angular;
            positionsTemp.AddLast(positionInTime);
        }        
        else
        {
            Debug.Log("no more past");
        }
    }

    protected virtual void UnRewind()
    {
        if (positionsTemp.Count > 0)
        {
            PositionInTime positionInTime = positionsTemp.Last.Value;
            positionsTemp.RemoveLast();
            transform.SetPositionAndRotation(positionInTime.position, positionInTime.rotation);
            speed = positionInTime.speed;
            angular = positionInTime.angular;
            positions.AddLast(positionInTime);
        }
        else
        {
            Debug.Log("no more future");
        }
    }

    protected void ApplyMovement()
    {
        rigidbody2D.velocity = speed;
        rigidbody2D.angularVelocity = angular;
    }
}

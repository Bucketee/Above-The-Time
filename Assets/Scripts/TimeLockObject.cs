using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockObject : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    [Header("Time Lock")]
    [SerializeField] private bool timeLocked = false;
    public bool TimeLocked => timeLocked;
    [SerializeField] private float duration = 10f;
    private Stack<PositionInTime> positions = new Stack<PositionInTime>();
    private Stack<PositionInTime> positionsTemp = new Stack<PositionInTime>(); //for future

    private Vector2 speed;
    private Quaternion rotation;
    private Coroutine nowTimeLockCoroutine = null;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponentInParent<Bullet>();
        if (bullet && bullet.type == Bullet.BulletType.time)
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
    }

    private void Update()
    {
        
        if (timeLocked)
        {
            float timeAmount = Input.GetAxis("Mouse ScrollWheel");
            if (timeAmount < 0)
            {
                for (int i = 0; i < (0.05f / Time.deltaTime); i++) Rewind();
            }
            else if (timeAmount > 0)
            {
                for (int i = 0; i < (0.05f / Time.deltaTime); i++) UnRewind();
            }
            return;
        }
        Record();
    }

    private IEnumerator TimeLockDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        GetTimeUnLocked();
    }

    private void Record()
    {
        if (timeLocked) return;
        positions.Push(new PositionInTime(transform.position, transform.rotation, speed));
    }

    private void GetTimeLocked()
    {
        Debug.Log("Locked!!");
        GameManager.Instance.TimeManager.SetNowTimeLockedObject(this);
        timeLocked = true;
        rigidbody2D.velocity = new Vector2(0, 0);
        rigidbody2D.angularVelocity = 0f;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        nowTimeLockCoroutine = StartCoroutine(TimeLockDuration(duration));
    }

    public void GetTimeUnLocked()
    {
        Debug.Log("UnLocked!!");
        GameManager.Instance.TimeManager.UnSetNowTimeLockedObject();
        timeLocked = false;
        positionsTemp = new Stack<PositionInTime>();
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        ApplyMovement();
    }

    private void Rewind()
    {
        if (positions.Count > 0)
        {
            PositionInTime positionInTime = positions.Pop();
            transform.SetPositionAndRotation(positionInTime.position, positionInTime.rotation);
            speed = positionInTime.speed;
            positionsTemp.Push(positionInTime);
        }        
        else
        {
            Debug.Log("no more past");
        }
    }

    private void UnRewind()
    {
        if (positionsTemp.Count > 0)
        {
            PositionInTime positionInTime = positionsTemp.Pop();
            transform.SetPositionAndRotation(positionInTime.position, positionInTime.rotation);
            speed = positionInTime.speed;
            positions.Push(positionInTime);
        }
        else
        {
            Debug.Log("no more future");
        }
    }

    private void ApplyMovement()
    {
        rigidbody2D.velocity = speed;
        rigidbody2D.rotation = rotation.z;
    }
}

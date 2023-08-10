using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFragment : TimeLockObject
{
    private Wall wallObject;
    private bool rewinding = false;
    private Vector2 firstPos;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        wallObject = GetComponentInParent<Wall>();
        firstPos = rigidbody2D.position;
    }

    private void Update()
    {
        if (timeLocked || rewinding) return;
        Record();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.collider.GetComponentInParent<Bullet>();
        if (bullet && bullet.type == Bullet.BulletType.time)
        {
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

    protected override void Record()
    {
        if (wallObject.timeLocked) return;
        positions.AddLast(new PositionInTime(transform.position, transform.rotation, GetComponent<Rigidbody2D>().velocity, GetComponent<Rigidbody2D>().angularVelocity));
    }

    public void ExplosionEffect(Vector2 loc, float power)
    {
        GetComponent<Rigidbody2D>().AddForceAtPosition((GetComponent<Rigidbody2D>().position - loc) * power / (GetComponent<Rigidbody2D>().position - loc).magnitude, loc, ForceMode2D.Impulse);
        Debug.Log("exploding");
    }

    public void Vibrating()
    {
        Debug.Log("vibrate");
        //help
    }

    public void GoToFirstPos()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(GoToFirstPosCo());
        Debug.Log("go to first");
    }

    private IEnumerator GoToFirstPosCo()
    {
        rewinding = true;
        while (positions.Count > 0)
        {
            Debug.Log("go to first");
            Rewind();
            yield return new WaitForSeconds(1f / (50 * 300 * Time.deltaTime));
        }
        transform.position = firstPos;
        rewinding = false;
    }

    public override void GetTimeLocked()
    {
        Debug.Log("Locked!!");
        timeLocked = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    public override void GetTimeUnLocked()
    {
        Debug.Log("UnLocked!!");
        GameManager.Instance.TimeManager.UnSetNowTimeLockedObject();
        timeLocked = false;
        positionsTemp.Clear();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        ApplyMovement();
    }
}
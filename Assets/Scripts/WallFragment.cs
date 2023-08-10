using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFragment : TimeLockObject
{
    private Wall wallObject;
    private bool rewinding = false;
    private Vector2 firstPos;
    private float rewindingSpeed;

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
        if (rewinding || wallObject.rewinding) return;
        Bullet bullet = collision.collider.GetComponentInParent<Bullet>();
        if (bullet && bullet.type == Bullet.BulletType.time)
        {
            if (timeLocked)
            {
                wallObject.TimeUnLocked();
            }
            else
            {
                wallObject.TimeLocked();
            }
        }
    }

    protected override void Record()
    {
        if (wallObject.timeLocked || gameObject.layer == LayerMask.NameToLayer("Wall") || rigidbody2D.velocity == new Vector2(0, 0)) return;
        positions.AddLast(new PositionInTime(transform.position, transform.rotation, rigidbody2D.velocity, rigidbody2D.angularVelocity));
        Debug.Log("recording");
    }

    public void ExplosionEffect(Vector2 loc, float power)
    {
        GetComponent<Rigidbody2D>().AddForceAtPosition((GetComponent<Rigidbody2D>().position - loc) * power / (GetComponent<Rigidbody2D>().position - loc).magnitude, loc, ForceMode2D.Impulse);
        Debug.Log("exploding");
    }

    public void Vibrating()
    {
        //Debug.Log("vibrate");
        //help
    }

    public void GoToFirstPos()
    {
        StartCoroutine(GoToFirstPosCo());
        Debug.Log("go to first");
    }

    private IEnumerator GoToFirstPosCo()
    {
        rewinding = true;
        rigidbody2D.velocity = new Vector2(0f, 0f);
        rigidbody2D.angularVelocity = 0f;
        while (positions.Count > 0)
        {
            Rewind();
            Debug.Log("rewinding");
            yield return new WaitForSeconds(1f / (50 * rewindingSpeed * Time.deltaTime));
        }
        transform.position = firstPos;
        speed = new Vector2(0f, 0f);
        angular = 0f;
        positions = new LinkedList<PositionInTime>();
        rewinding = false;
        wallObject.AddRewindCount();
    }

    public override void GetTimeLocked()
    {
        //Debug.Log("Locked!!");
        GameManager.Instance.TimeManager.SetNowTimeLockedObject(this);
        timeLocked = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
    }

    public override void GetTimeUnLocked()
    {
        //Debug.Log("UnLocked!!");
        GameManager.Instance.TimeManager.UnSetNowTimeLockedObject();
        timeLocked = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        ApplyMovement();
    }

    public void SetRewindSpeed(float speed)
    {
        rewindingSpeed = speed;
    }
}
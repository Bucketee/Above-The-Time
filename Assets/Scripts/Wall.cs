using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("State")]
    public bool broken;
    public bool rewinding = false;
    public bool timeLocked = false;
    public float count;
    [SerializeField] private float amount;

    [Header("Fragments")]
    [SerializeField] private List<WallFragment> fragments = new List<WallFragment>();
    private int rewindCount = 0;
    [SerializeField] private float speed = 800f;

    [Header("Explosion")]
    [SerializeField] private Vector2 explosionLoc;
    public float power; //power of breaking walls

    private void Awake()
    {
        count = 0;
        explosionLoc = transform.GetChild(0).transform.position;
        foreach (WallFragment fragment in GetComponentsInChildren<WallFragment>())
        {
            fragments.Add(fragment);
            fragment.SetRewindSpeed(speed);
        }
    }

    private void Start()
    {
        if (broken)
        {
            StartCoroutine(BreakWallCo(true, power));
        }
    }

    private void Update()
    {
        if (timeLocked)
        {
            float timeAmount = Input.GetAxis("Mouse ScrollWheel");
            if (count * timeAmount > 0)
            {
                Vibrating();
                count += timeAmount;
            }
            if (broken && count < -amount)
            {
                StopVibrating();
                FixWall();
            }
            else if (!broken && count > amount)
            {
                StopVibrating();
                BreakWall(true, power);
            }
            return;
        }
    }

    /// <summary>
    /// break wall by explosion effect
    /// </summary>
    /// <param name="isQuiet">true to be used at start of the game</param>
    public void BreakWall(bool isQuiet, float power) //help sound
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            fragment.gameObject.layer = LayerMask.NameToLayer("WallFragment");
            fragment.ExplosionEffect(explosionLoc, power);
        }
        broken = true;
    }

    /// <summary>
    /// only for starting of the game
    /// </summary>
    /// <param name="isQuiet"></param>
    /// <param name="power"></param>
    /// <returns></returns>
    private IEnumerator BreakWallCo(bool isQuiet, float power)
    {
        yield return new WaitForSeconds(0.06f);
        BreakWall(isQuiet, power);
    }

    private void FixWall()
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fragment.GoToFirstPos();
        }
        rewindCount = 0;
        rewinding = true;
        broken = false;
    }

    public void TimeLocked()
    {
        if (timeLocked) return;
        count = broken ? -1 : 1;
        timeLocked = true;
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetTimeLocked();
        }
    }

    public void TimeUnLocked()
    {
        if (!timeLocked) return;
        count = broken ? -1 : 1;
        timeLocked = false;
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetTimeUnLocked();
        }
    }

    private void Vibrating()
    {
        foreach (WallFragment fragment in fragments)
        {
            fragment.Vibrating();
        }
    }

    private void StopVibrating()
    {
        foreach (WallFragment fragment in fragments)
        {
            fragment.StopVibrating();
        }
    }

    public void AddRewindCount()
    {
        rewindCount += 1;
        if (rewindCount == fragments.Count)
        {
            rewinding = false;
        }
    }
}
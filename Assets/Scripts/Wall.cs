using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("State")]
    public bool broken;
    [SerializeField] private Vector2 explosionLoc;
    public bool timeLocked = false;
    [SerializeField] private float count;
    [SerializeField] private float amount;

    [Header("Fragments")]
    [SerializeField] private List<WallFragment> fragments = new List<WallFragment>();
    public bool rewinding = false;
    private int rewindCount = 0;
    public float power; //power of breaking walls
    [SerializeField] private float speed = 500;

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
            BreakWall(true, power);
        }
    }

    private void Update()
    {
        if (timeLocked)
        {
            float timeAmount = Input.GetAxis("Mouse ScrollWheel");
            count += timeAmount;
            if (count * timeAmount > 0) Vibrating();
            if (broken && count < -amount)
            {
                FixWall();
            }
            else if (!broken && count > amount)
            {
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

    private void FixWall()
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fragment.gameObject.layer = LayerMask.NameToLayer("Wall");
            fragment.GoToFirstPos();
        }
        rewindCount = 0;
        rewinding = true;
        broken = false;
    }

    public void TimeLocked()
    {
        count = 0;
        timeLocked = true;
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetTimeLocked();
        }
    }

    public void TimeUnLocked()
    {
        count = 0;
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

    public void AddRewindCount()
    {
        rewindCount += 1;
        if (rewindCount == fragments.Count)
        {
            rewinding = false;
        }
    }
}
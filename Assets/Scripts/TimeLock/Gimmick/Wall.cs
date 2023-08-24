using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("State")]
    public bool broken = false;
    public bool rewinding = false;
    public bool timeLocked = false;
    public float count;
    public float amount;

    [Header("Fragments")]
    [SerializeField] protected  List<WallFragment> fragments = new List<WallFragment>();
    protected int rewindCount = 0;
    [SerializeField] protected float recoveringSpeed = 80f;

    [Header("Explosion")]
    [SerializeField] protected Vector2 explosionLoc;
    public float explosionPower; //power of breaking walls
    private GameStateManager gameStateManager;

    private TimeZoneWall timeZoneWall;
    private void Awake()
    {
        timeZoneWall = GetComponent<TimeZoneWall>();
        count = 0;
        explosionLoc = transform.GetChild(0).transform.position;
        foreach (WallFragment fragment in GetComponentsInChildren<WallFragment>())
        {
            fragments.Add(fragment);
            fragment.SetRewindSpeed(recoveringSpeed);
        }
    }

    private void Start()
    {
        if (broken)
        {
            StartCoroutine(BreakWallCo(true, explosionPower));
        }
        gameStateManager = GameManager.Instance.GameStateManager;
    }

    private void Update()
    {
        if (gameStateManager.NowGameState != GameState.Playing)
        {
            return;
        }
        
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
                broken = false;
                timeZoneWall.Record(GameManager.Instance.TimeZoneManager.NowTimeZone);
                FixWall();
            }
            else if (!broken && count > amount)
            {
                StopVibrating();
                broken = true;
                timeZoneWall.Record(GameManager.Instance.TimeZoneManager.NowTimeZone);
                BreakWall(true, explosionPower);
            }
            return;
        }
    }

    /// <summary>
    /// break wall by explosion effect
    /// </summary>
    /// <param name="isQuiet">true to be used at start of the game</param>
    public virtual void BreakWall(bool isQuiet, float power) //help sound
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            fragment.gameObject.layer = LayerMask.NameToLayer("WallFragment");
            fragment.ExplosionEffect(explosionLoc, power);
        }
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

    protected virtual void FixWall()
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fragment.GoToFirstPos();
        }
        rewindCount = 0;
        rewinding = true;
    }

    public virtual void InitializeWall()
    {
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fragment.GoToFirstPosNow();
        }
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

    protected void Vibrating()
    {
        foreach (WallFragment fragment in fragments)
        {
            fragment.Vibrating();
        }
    }

    protected void StopVibrating()
    {
        foreach (WallFragment fragment in fragments)
        {
            fragment.StopVibrating();
        }
    }

    public virtual void AddRewindCount()
    {
        rewindCount += 1;
        if (rewindCount == fragments.Count)
        {
            rewindCount = 0;
            rewinding = false;
        }
    }

    public void SetFragSortingLayer(string layer)
    {
        foreach (WallFragment fragment in fragments)
        {
            fragment.SetSortingLayer(layer);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockLever : TimeLockObject
{
    private Lever lever;
    private TimeZoneLever timeZoneLever;
    public LinkedList<Lever.LeverState> leverStates = new();
    public LinkedList<Lever.LeverState> leverStatesTemp = new();

    [Header("State")]
    public float count;
    public float amount = 20;

    private void Awake()
    {
        lever = GetComponent<Lever>();
        timeZoneLever = GetComponent<TimeZoneLever>();
    }

    private void Start()
    {
        gameStateManager = GameManager.Instance.GameStateManager;
    }

    private void Update()
    {
        if (gameStateManager.NowGameState != GameState.Playing)
        {
            return;
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    string str = "";
        //    foreach (Lever.LeverState s in leverStates)
        //    {
        //        str += (int)s;
        //    }
        //    Debug.Log(str);
        //    str = "";
        //    foreach (Lever.LeverState s in leverStatesTemp)
        //    {
        //        str += (int)s;
        //    }
        //    Debug.Log(str);
        //}
        if (timeLocked)
        {
            float timeAmount = Input.GetAxis("Mouse ScrollWheel");
            if (count * timeAmount > 0)
            {
                count += timeAmount;
            }
            else if (count * timeAmount < 0)
            {
                count = -count / Mathf.Abs(count);
            }
            if (count > amount)
            {
                UnRewind();
            }
            else if (count < -amount)
            {
                Rewind();
            }
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<MouseCursor>(out MouseCursor mouse)) return;
        SetSortingLayer("Timelockable");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<MouseCursor>(out MouseCursor mouse)) return;
        SetSortingLayer("Default");
    }

    protected override void Record()
    {
        leverStates.AddLast(lever.leverState);
    }

    public override void GetTimeLocked()
    {
        Debug.Log("Locked!!");
        GameManager.Instance.TimeManager.SetNowTimeLockedObject(this);
        timeLocked = true;
        count = -1;
        nowTimeLockCoroutine = StartCoroutine(TimeLockDuration(duration));
    }

    public override void GetTimeUnLocked()
    {
        Debug.Log("UnLocked!!");
        GameManager.Instance.TimeManager.UnSetNowTimeLockedObject();
        timeLocked = false;
        count = 0;
        if (nowTimeLockCoroutine != null)
        {
            StopCoroutine(nowTimeLockCoroutine);
            nowTimeLockCoroutine = null;
        }
        SetSortingLayer("Default");
    }

    protected override void Rewind()
    {
        if (leverStates.Count > 0)
        {
            leverStatesTemp.AddLast(lever.leverState);
            Lever.LeverState leverState = leverStates.Last.Value;
            leverStates.RemoveLast();
            lever.SetLeverState(leverState);
        }
        else
        {
            Debug.Log("no more past");
        }
        lever.OnStateChange();
        count = -1;
    }

    protected override void UnRewind()
    {
        if (leverStatesTemp.Count > 0)
        {
            leverStates.AddLast(lever.leverState);
            Lever.LeverState leverState = leverStatesTemp.Last.Value;
            leverStatesTemp.RemoveLast();
            lever.SetLeverState(leverState);
        }
        else if ((int) lever.leverState <= 1)
        {
            leverStates.AddLast(lever.leverState);
            lever.SetLeverState(lever.leverState + 2);
        }
        else
        {
            Debug.Log("no more future");
        }
        lever.OnStateChange();
        count = 1;
    }

    public void Interacted()
    {
        Record();
        leverStatesTemp.Clear();
    }
}

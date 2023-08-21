using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractionObject
{
    private TimeLockLever timeLockLever;
    public LeverState leverState;

    public enum LeverState
    {
        Off,
        On,
        OffBroken,
        OnBroken,
    }

    private void Awake()
    {
        timeLockLever = GetComponent<TimeLockLever>();
    }

    private void Start()
    {
        if ((int) leverState <= 1)
        {

        }
        else
        {
            timeLockLever.leverStates.AddLast(leverState - 2);
        }
    }

    public override void Interact()
    {
        if ((int) leverState <= 1)
        {
            if (timeLockLever.TimeLocked) timeLockLever.GetTimeUnLocked();
            timeLockLever.Interacted();
            SetLeverState(1 - leverState);
        }
        else
        {
            GameManager.Instance.UIManager.AddText("<color=#EC591A>A broken lever cannot be controlled</color>", 3);
        }
    }

    public void SetLeverState(LeverState state)
    {
        leverState = state;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterInputController", menuName = "inputController/MonsterAInputController")]
public class MonsterAInputController : InputController
{
    public override float RetrieveMoveInput()
    {
        if (1.5f < Mathf.PingPong(Time.time, 3f))
        {
            return 1f;
        }
        return -1f;
    }

    public override bool RetrieveJumpInput()
    {
        if (Time.time > 5f)
        {
            return true;
        }
        return false;
    }

    public override bool RetrieveJumpHoldInput()
    {
        return false;
    }

    public override bool RetrieveJumpUpInput()
    {
        return false;
    }

    public override bool RetrieveAttackInput()
    {
        return false;
    }
}

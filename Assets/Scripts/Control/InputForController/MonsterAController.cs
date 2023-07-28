using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterController", menuName = "inputController/MonsterAController")]
public class MonsterAController : InputController
{
    public override float RetrieveMoveInput()
    {
        if (1.5 < Mathf.PingPong(Time.time, 3))
        {
            return 1;
        }
        return -1;
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

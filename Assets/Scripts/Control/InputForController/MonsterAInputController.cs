using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterInputController", menuName = "inputController/MonsterAInputController")]
public class MonsterAInputController : InputController
{
    public override float RetrieveMoveInput()
    {
        return 0f;
    }

    public override bool RetrieveJumpInput()
    {
        /*
        if (Time.time > 5f)
        {
            return true;
        }
        */
        return Input.GetKey(KeyCode.LeftControl);
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

    public override float RetrieveTimeDirInput()
    {
        return Input.GetAxis("Mouse ScrollWheel");
    }
}

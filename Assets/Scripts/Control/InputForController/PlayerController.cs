using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "inputController/PlayerController")]
public class PlayerController : InputController
{
    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveRunningInput()
    {
        return Input.GetKey(KeyCode.LeftShift); //help
    }

    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override bool RetrieveJumpHoldInput()
    {
        return Input.GetButton("Jump");
    }

    public override bool RetrieveJumpUpInput()
    {
        return Input.GetButtonUp("Jump");
    }

    public override bool RetrieveAttackInput()
    {
        return Input.GetKey(KeyCode.Z); //help
    }
}

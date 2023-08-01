using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputController", menuName = "inputController/PlayerInputController")]
public class PlayerInputController : InputController
{
    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override bool RetrieveDashInput()
    {
        return Input.GetKey(KeyCode.LeftShift);
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
        return false;
    }
}

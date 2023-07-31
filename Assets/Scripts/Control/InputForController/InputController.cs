using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();

    public virtual bool RetrieveDashInput()
    {
        return false;
    }

    public abstract bool RetrieveJumpInput();

    public abstract bool RetrieveJumpHoldInput();

    public abstract bool RetrieveJumpUpInput();

    public abstract bool RetrieveAttackInput();

    /// <summary>
    /// go to future if 1, past if -1, just stop (do nothing) if 0
    /// </summary>
    /// <returns></returns>
    public virtual float RetrieveTimeDirInput()
    {
        return 0;
    }
}

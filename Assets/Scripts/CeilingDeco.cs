using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDeco : Wall
{
    public override void BreakWall(bool isQuiet, float power) //help sound
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<CeilingDecoFrag>().joint2D.enabled = false;
            fragment.ExplosionEffect(explosionLoc, power);
        }
        broken = true;
    }

    protected override void FixWall()
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fragment.GoToFirstPos();
        }
        rewindCount = 0;
        rewinding = true;
        broken = false;
    }

    public override void AddRewindCount()
    {
        rewindCount += 1;
        if (rewindCount == fragments.Count)
        {
            rewinding = false;
            foreach (WallFragment fragment in fragments)
            {
                fragment.GetComponent<CeilingDecoFrag>().joint2D.enabled = true;
                fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}

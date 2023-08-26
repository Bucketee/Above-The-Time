using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingDeco : Wall
{
    private TimeZoneCeilingDeco timeZoneCeilingDeco;

    private void Awake()
    {
        timeZoneCeilingDeco = GetComponent<TimeZoneCeilingDeco>();
        count = 0;
        explosionLoc = transform.GetChild(0).transform.position;
        foreach (WallFragment fragment in GetComponentsInChildren<WallFragment>())
        {
            fragments.Add(fragment);
            fragment.SetRewindSpeed(recoveringSpeed);
        }
    }

    private void Update()
    {
        if (timeLocked && GameManager.Instance.TimeManager.timeWindCost > 0)
        {
            float timeAmount = Input.GetAxis("Mouse ScrollWheel");
            if (count * timeAmount > 0)
            {
                GameManager.Instance.TimeManager.timeWindCost -= 3f;
                Vibrating();
                count += timeAmount;
            }
            if (broken && count < -amount)
            {
                StopVibrating();
                broken = false;
                timeZoneCeilingDeco.Record(GameManager.Instance.TimeZoneManager.NowTimeZone);
                FixWall();
            }
            else if (!broken && count > amount)
            {
                StopVibrating();
                broken = true;
                timeZoneCeilingDeco.Record(GameManager.Instance.TimeZoneManager.NowTimeZone);
                BreakWall(true, explosionPower);
            }
            return;
        }
    }

    public override void BreakWall(bool isQuiet, float power) //help sound
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<CeilingDecoFrag>().joint2D.enabled = false;
            fragment.ExplosionEffect(explosionLoc, power);
            fragment.gameObject.layer = LayerMask.NameToLayer("CeilingDeco");
        }
    }

    protected override void FixWall()
    {
        TimeUnLocked();
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fragment.GoToFirstPos();
            fragment.gameObject.layer = fragment.GetComponent<CeilingDecoFrag>().initLayer;
        }
        rewindCount = 0;
        rewinding = true;
    }

    public override void AddRewindCount()
    {
        rewindCount += 1;
        if (rewindCount == fragments.Count)
        {
            rewindCount = 0;
            rewinding = false;
            foreach (WallFragment fragment in fragments)
            {
                fragment.GetComponent<CeilingDecoFrag>().joint2D.enabled = true;
                fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    public override void InitializeWall()
    {
        foreach (WallFragment fragment in fragments)
        {
            fragment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            fragment.GoToFirstPosNow();
        }
    }
}

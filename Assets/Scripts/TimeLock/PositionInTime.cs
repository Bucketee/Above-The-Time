using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInTime
{
    public Vector2 position;
    public Quaternion rotation;
    public Vector2 speed;
    public float angular;

    public PositionInTime(Vector2 position, Quaternion rotation, Vector2 speed, float angular)
    {
        this.position = position;
        this.rotation = rotation;
        this.speed = speed;
        this.angular = angular;
    }
}

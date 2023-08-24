using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Border
{
    [SerializeField] public string borderName;
    [SerializeField] public float minX, minY, maxX, maxY;

    public Border(string borderName, float minX, float minY, float maxX, float maxY)
    {
        this.borderName = borderName;
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
    }
}
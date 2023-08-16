using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class TimeZoneObject : MonoBehaviour
{
    public Dictionary<TimeZone, bool> isTriggerDict = new();
    public Dictionary<TimeZone, Vector3> posDict = new();
    public Dictionary<TimeZone, Sprite> spriteDict = new();
    public Dictionary<TimeZone, float> rotDict = new();
    [SerializeField] private TimeZoneObjectInfo timeZoneObjectInfo;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;

    public void Settting()
    {
        TimeZone timeZone = GameManager.Instance.TimeZoneManager.NowTimeZone;
        Rigidbody2D rigidbody2D;
        if (TryGetComponent<Rigidbody2D>(out rigidbody2D))
        {
            rigidbody2D.velocity = new Vector2(0f, 0f);
            rigidbody2D.angularVelocity = 0f;
            rigidbody2D.position = posDict[timeZone];
            rigidbody2D.rotation = rotDict[timeZone];
            if (isTriggerDict[timeZone])
            {
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                rigidbody2D.constraints = RigidbodyConstraints2D.None;
            }
        }
        else
        {
            gameObject.transform.position = posDict[timeZone];
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotDict[timeZone]));
        }
        spriteRenderer.sprite = spriteDict[timeZone];
        polygonCollider2D.isTrigger = isTriggerDict[timeZone];
        
        
        Collider2DExtensions.TryUpdateShapeToAttachedSprite(polygonCollider2D); //모든 Collider가 PolyGon이라 가정
    }

    public void SetDictionary<T>(Dictionary<TimeZone, T> dictionary, TimeZone timeZone, T data)
    {
        dictionary[timeZone] = data;
    }

    private void Start()
    {
        foreach(TimeZoneV3Active t in timeZoneObjectInfo.timeZoneV3s)
        {
            posDict.Add(t.timeZone, t.pos);
            isTriggerDict.Add(t.timeZone, t.isTriggered);
            spriteDict.Add(t.timeZone, t.sprite);
            rotDict.Add(t.timeZone, t.rot);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    [Serializable]
    public class TimeZoneObjectInfo
    {
        public List<TimeZoneV3Active> timeZoneV3s;
    }

    [Serializable]
    public class TimeZoneV3Active
    {
        public TimeZone timeZone;
        public Vector3 pos;
        public float rot;
        public bool isTriggered;
        public Sprite sprite;
    }
}

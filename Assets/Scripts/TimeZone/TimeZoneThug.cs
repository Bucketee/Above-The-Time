using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeZoneThug : MonoBehaviour, ITimeZoneInterface
{
    public Dictionary<TimeZone, bool> isTriggerDict = new();
    public Dictionary<TimeZone, Sprite> spriteDict = new();
    public Dictionary<TimeZone, Vector3> posDict = new();

    private TimeZoneManager timeZoneManager;
    [SerializeField] private TimeZoneObjectInfo timeZoneObjectInfo;
    [SerializeField] private Sprite emptySprite;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;
    private ThugController thugController;

    private void Start()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;

        foreach(TimeZoneV3Active t in timeZoneObjectInfo.timeZoneV3s)
        {
            if (!posDict.ContainsKey(t.timeZone))
            {
                posDict.Add(t.timeZone, t.pos);
            }

            if (!isTriggerDict.ContainsKey(t.timeZone))
            {
                isTriggerDict.Add(t.timeZone, t.isTriggered);
            }

            if (!spriteDict.ContainsKey(t.timeZone))
            {
                spriteDict.Add(t.timeZone, t.sprite);
            }
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        thugController = GetComponent<ThugController>();
        Setting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Die(timeZoneManager.NowTimeZone);
            Setting();
        }
    }

    public void Setting()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;

        TimeZone timeZone = GameManager.Instance.TimeZoneManager.NowTimeZone;
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            rigidbody2D.velocity = new Vector2(0f, 0f);
            rigidbody2D.angularVelocity = 0f;
            rigidbody2D.position = posDict[timeZone];
            if (isTriggerDict[timeZone])
            {
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else
        {
            gameObject.transform.position = posDict[timeZone];
        }

        spriteRenderer.sprite = spriteDict[timeZone]; 
        polygonCollider2D.isTrigger = isTriggerDict[timeZone];   

        Collider2DExtensions.TryUpdateShapeToAttachedSprite(polygonCollider2D);    
        thugController.ResetMove();
    }

    public void Die(TimeZone timeZone)
    {
        foreach(TimeZone t in Enum.GetValues(typeof(TimeZone)))
        {
            if ((int) t >= (int) timeZone)
            {
                isTriggerDict[t] = true;
                spriteDict[t] = emptySprite;
            }
        }

        Setting();
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
        public bool isTriggered;
        public Sprite sprite;
    }
}

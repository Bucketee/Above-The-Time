using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneNormalObject : MonoBehaviour, ITimeZoneInterface
{
    public Dictionary<TimeZone, bool> isTriggerDict = new();
    public Dictionary<TimeZone, Vector3> posDict = new();
    public Dictionary<TimeZone, Sprite> spriteDict = new();
    public Dictionary<TimeZone, float> rotDict = new();
    [SerializeField] private TimeZoneObjectInfo timeZoneObjectInfo;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;
    private TimeZoneManager timeZoneManager;

    public void Setting()
    {
        TimeZone timeZone = GameManager.Instance.TimeZoneManager.NowTimeZone;
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
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

    private void Awake()
    {
        foreach(TimeZoneV3Active t in timeZoneObjectInfo.timeZoneV3s)
        {
            if (!posDict.ContainsKey(t.timeZone))
            {
                posDict.Add(t.timeZone, t.pos);
            }

            if (!rotDict.ContainsKey(t.timeZone))
            {
                rotDict.Add(t.timeZone, t.rot);
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

    }
    private void Start()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;
    }

    private void Record(TimeZone timeZone)
    {
        foreach(TimeZone t in Enum.GetValues(typeof(TimeZone)))
        {
            if ((int) t >= (int) timeZone)
            {
                posDict[t] = transform.position;
                rotDict[t] = transform.rotation.eulerAngles.z;
            }
        }
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            Record(timeZoneManager.NowTimeZone);
            transform.hasChanged = false;
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senior : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TimeZoneManager timeZoneManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;
    }

    void Update()
    {
        if (timeZoneManager.NowTimeZone == TimeZone.Present)
        {
            Destroy(gameObject);
        }
    }
}

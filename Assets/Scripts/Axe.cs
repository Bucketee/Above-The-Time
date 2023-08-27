using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private TimeZoneManager timeZoneManager;
    private StoryManager storyManager;
    private TimeLockAxe timeLockAxe;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider2d;

    private void Start()
    {
        timeZoneManager = GameManager.Instance.TimeZoneManager;
        storyManager = GameManager.Instance.StoryManager;
        timeLockAxe = GetComponent<TimeLockAxe>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (timeZoneManager.NowTimeZone == TimeZone.Present && storyManager.CurrentStory == StoryProgress.ChaseAxe)
        {
            timeLockAxe.enabled = true;
            spriteRenderer.enabled = true;
            collider2d.enabled = true;
        }
        else
        {
            timeLockAxe.enabled = false;
            spriteRenderer.enabled = false;
            collider2d.enabled = false;
        }
    }

}

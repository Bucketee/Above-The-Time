using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlumBackground : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;

    private TimeZoneManager timeZoneManager;
    private StoryManager storyManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeZoneManager = GameManager.Instance.TimeZoneManager;
        storyManager = GameManager.Instance.StoryManager;
    }

    private void Update()
    {
        if (timeZoneManager.NowTimeZone == TimeZone.Past)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (timeZoneManager.NowTimeZone == TimeZone.Present)
        {
            spriteRenderer.sprite = sprites[1];
        }
        else if (timeZoneManager.NowTimeZone == TimeZone.Future)
        {
            if (storyManager.CurrentStory < StoryProgress.BackToTree)
            {
                spriteRenderer.sprite = sprites[2];
            }
            else
            {
                spriteRenderer.sprite = sprites[0];
            }
        }
    }
}

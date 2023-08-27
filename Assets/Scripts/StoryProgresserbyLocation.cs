using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgresserbyLocation : MonoBehaviour
{
    [SerializeField] private StoryProgress current;
    [SerializeField] private float timezone;
    private StoryManager storyManager;

    private void Start()
    {
        storyManager = GameManager.Instance.StoryManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (timezone == 0 || (timezone != 0 && timezone == 1 + (int)GameManager.Instance.TimeZoneManager.NowTimeZone))
        {
            storyManager.NextStory();
            Destroy(gameObject);
        }
    }
}

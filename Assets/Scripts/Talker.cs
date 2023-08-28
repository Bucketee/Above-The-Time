using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour
{
    [SerializeField] private StoryProgress availablestory;
    [SerializeField] private int talkNum;
    
    [SerializeField] private float timezone;
    [SerializeField] private bool nextstory;
    private StoryManager storyManager;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        storyManager = GameManager.Instance.StoryManager;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player") || (storyManager.CurrentStory != availablestory && (int)storyManager.CurrentStory <= 11)) return;
        if (timezone == 0 || (timezone != 0 && timezone == 1 + (int)GameManager.Instance.TimeZoneManager.NowTimeZone))
        {
            GameManager.Instance.TalkManager.StartTalk(talkNum);
            if (nextstory) GameManager.Instance.StoryManager.NextStory();
            Destroy(gameObject);
        }
        
    }
}

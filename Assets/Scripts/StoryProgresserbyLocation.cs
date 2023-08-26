using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryProgresserbyLocation : MonoBehaviour
{
    [Tooltip("Go to this story when triggered")]
    [SerializeField] private StoryProgress nextStory;
    private StoryManager storyManager;

    private void Start()
    {
        storyManager = GameManager.Instance.StoryManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (storyManager.CurrentStory < nextStory)
        {
            storyManager.SelectStory(nextStory);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTree : InteractionObject
{
    [SerializeField] GameObject tree;

    private void Start()
    {
        if (GameManager.Instance.StoryManager.CurrentStory > StoryProgress.PlantTree)
        {
            Destroy(gameObject);
        }
    }

    public override void Interact()
    {
        //GameManager.Instance.StoryManager.SelectStory(StoryProgress.PresentSlum);
        Vector3 pos = transform.position;
        Instantiate(tree, pos, Quaternion.identity);
        Destroy(gameObject);
    }
}

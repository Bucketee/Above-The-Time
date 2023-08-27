using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.StoryManager.CurrentStory >= StoryProgress.PlantTree)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mover : InteractionObject
{
    public Vector3 location;
    [SerializeField] private bool available;
    [SerializeField] Player player;

    public override void Interact()
    {
        if (!available) Checkavailable();
        if (available)
        {
            player.transform.position = location;
        }
        else
        {
            GameManager.Instance.UIManager.AddText("<color=#EC591A>Catch Boss to go</color>", 3);
        }
    }

    private void Checkavailable()
    {
        if (gameObject.name.Equals("ToTopTower"))
        {
            available = true;
            GameManager.Instance.StoryManager.SelectStory(StoryProgress.TopTower);
            return;
        }
        if (gameObject.name.Equals("ToButtomTower") && GameManager.Instance.StoryManager.CurrentStory >= StoryProgress.BackToTree)
        {
            available = true;
            return;
        }

    }
}

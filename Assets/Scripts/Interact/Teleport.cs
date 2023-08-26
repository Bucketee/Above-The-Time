using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : InteractionObject
{
    public string sceneName;
    [SerializeField] private bool available;

    public override void Interact()
    {
        if (!available) Checkavailable();
        if (available)
        {
            Debug.Log(sceneName);
            GameManager.Instance.StoryManager.NextStory();
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            GameManager.Instance.UIManager.AddText("<color=#EC591A>You cant go to " + sceneName + "</color>", 3);
        }
    }

    private void Checkavailable()
    {
        if (gameObject.name.Equals("SlumToTower") && GameManager.Instance.StoryManager.CurrentStory >= StoryProgress.ChaseAxe)
        {
            available = true;
            return;
        }
        if (gameObject.name.Equals("TowerToSlum") && GameManager.Instance.StoryManager.CurrentStory >= StoryProgress.BackToTree)
        {
            available = true;
            return;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : InteractionObject
{
    public string sceneName;
    private bool available = false;

    public override void Interact()
    {
        if (!available) Checkavailable();
        if (available)
        {
            Debug.Log(sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            GameManager.Instance.UIManager.AddText("<color=#EC591A>You cant go to " + sceneName + "</color>", 3);
        }
    }

    private void Checkavailable()
    {
        if (gameObject.name.Equals("AssociationToSlum") && GameManager.Instance.StoryManager.CurrentStory == StoryProgress.Tutorial)
        {
            available = true;
            GameManager.Instance.StoryManager.SelectStory(StoryProgress.StartSlum);
            return;
        }
        if (gameObject.name.Equals("SlumToTower") && GameManager.Instance.StoryManager.CurrentStory >= StoryProgress.ChaseAxe)
        {
            available = true;
            GameManager.Instance.StoryManager.SelectStory(StoryProgress.InTower);
            return;
        }
        if (gameObject.name.Equals("TowerToSlum") && GameManager.Instance.StoryManager.CurrentStory >= StoryProgress.BackToTree)
        {
            available = true;
            return;
        }

    }
}

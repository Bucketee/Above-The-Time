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
}

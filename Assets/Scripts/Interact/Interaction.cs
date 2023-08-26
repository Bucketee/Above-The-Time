using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private UIManager uiManager;
    private new CircleCollider2D collider;

    [SerializeField] private List<GameObject> interactList = new();
    [SerializeField] private GameObject interactObject;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        uiManager = GameManager.Instance.UIManager;
    }

    private void Update()
    {
        if (interactList.Count > 0)
        {
            float distance = 100f;
            foreach (GameObject gameObject in interactList)
            {
                if ((gameObject.transform.position - transform.position).magnitude < distance)
                {
                    distance = (gameObject.transform.position - transform.position).magnitude;
                    interactObject = gameObject;
                }
            }
            if (interactObject.gameObject.layer == LayerMask.NameToLayer("Lever"))
            {
                uiManager.AddText("<color=black><size=36>" + "Press F to interact with </size>" + "<size=42><b>" + interactObject.name + "</b></size></color>");
            }
            else if (interactObject.gameObject.layer == LayerMask.NameToLayer("Teleport"))
            {
                uiManager.AddText("<color=black><size=36>" + "Press F to go to </size>" + "<size=42><b>" + interactObject.GetComponent<Teleport>().sceneName + "</b></size></color>");
            }
        }
        else
        {
            interactObject = null;
        }

        if (Input.GetKeyDown(KeyCode.F) && interactObject != null)
        {
            interactObject.GetComponent<InteractionObject>().Interact();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractionObject>() && !interactList.Contains(collision.gameObject)) interactList.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactList.Remove(collision.gameObject);
    }
}

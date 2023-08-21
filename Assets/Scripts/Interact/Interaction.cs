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
            uiManager.AddText("Press E to interact with " + interactObject.name);
        }
        else
        {
            interactObject = null;
        }

        if (Input.GetKeyDown(KeyCode.E) && interactObject != null)
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

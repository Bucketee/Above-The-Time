using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private new CircleCollider2D collider;

    private List<GameObject> interactList = new();

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            foreach(GameObject game in interactList)
            {
                Debug.Log(game);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractionObject>()) interactList.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        interactList.Remove(collision.gameObject); //wtf
    }
}

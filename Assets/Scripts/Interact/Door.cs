using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<Lever> levers = new();
    [Tooltip("off is 0, on is 1")]
    [SerializeField] private string password;

    [SerializeField] private bool open;
    private Coroutine coroutine;

    private float initY;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        initY = transform.position.y;
    }

    private void Update()
    {
        string cur = "";
        foreach (Lever lever in levers)
        {
            cur += (int)lever.leverState % 2;
        }
        if (password.Equals(cur))
        {
            open = true;
            OnOpen();
        }
        else
        {
            open = false;
            OnClose();
        }
    }

    private void OnOpen()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        StartCoroutine(move());
    }

    private void OnClose()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        StartCoroutine(move());
    }

    private IEnumerator move()
    {
        float targetY;
        if (open)
        {
            targetY = initY + 1.8f;
        }
        else
        {
            targetY = initY;
        }

        while (true)
        {
            yield return null;
            rigidbody2D.position = new Vector2(rigidbody2D.position.x, Mathf.MoveTowards(rigidbody2D.position.y, targetY, 0.0008f));
            if (rigidbody2D.position.y == targetY) break;
        }
    }
}

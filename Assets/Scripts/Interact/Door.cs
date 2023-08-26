using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<Lever> levers = new();
    [Tooltip("off is 0, on is 1")]
    [SerializeField] private string password;
    [SerializeField] private bool isHorizontal;
    [SerializeField] private bool negativeDir;
    [SerializeField] private float speed = 0.0008f;

    [SerializeField] private bool open;
    private Coroutine coroutine;

    private float init;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (isHorizontal)
        {
            init = transform.position.x;
        }
        else
        {
            init = transform.position.y;
        }
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
        float target;
        if (open)
        {
            target = init + 1.8f * transform.localScale.x * (negativeDir ? -1 : 1);
        }
        else
        {
            target = init;
        }

        while (true)
        {
            yield return null;
            if (isHorizontal)
            {
                rigidbody2D.position = new Vector2(Mathf.MoveTowards(rigidbody2D.position.x, target, speed), rigidbody2D.position.y);
                if (rigidbody2D.position.x == target) break;
            }
            else
            {
                rigidbody2D.position = new Vector2(rigidbody2D.position.x, Mathf.MoveTowards(rigidbody2D.position.y, target, speed));
                if (rigidbody2D.position.y == target) break;
            }
            
        }
    }
}

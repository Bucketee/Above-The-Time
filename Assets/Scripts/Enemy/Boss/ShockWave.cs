using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private float speed = 10f;
    private Coroutine destroyCo = null;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 dir)
    {
        rigidbody2D.velocity = dir * speed;
        destroyCo = StartCoroutine(DestroyCo());
    }

    private IEnumerator DestroyCo()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3) // Player
        {
            other.gameObject.GetComponentInParent<Player>().GetDamaged(20f);
        }
        else if (other.gameObject.layer == 20) // Rock
        {
            StopCoroutine(destroyCo);
            Destroy(this.gameObject);
        }
    }
}

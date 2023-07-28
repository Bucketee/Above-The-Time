using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    public BulletType type;
    public enum BulletType
    {
        normal,
        time,
    };

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Init(int type, Vector2 dir, float speed, float life)
    {
        this.type = (BulletType)type;
        rigidbody2D.velocity = dir * speed;
        StartCoroutine(bulletLength(life));
    }

    private IEnumerator bulletLength(float life)
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider)
        {
            Destroy(gameObject);
        }
    }

    public new BulletType GetType()
    {
        return type;
    }
}

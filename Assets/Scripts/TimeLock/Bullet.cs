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
    };

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Init(int type, Vector2 dir, float speed, float life)
    {
        this.type = (BulletType)type;
        rigidbody2D.velocity = dir * speed;
        StartCoroutine(BulletLength(life));
    }

    private IEnumerator BulletLength(float life)
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider)
        {
            if (collision.gameObject.layer == 7)
            {
                collision.gameObject.GetComponent<IEnemyInterface>().GetDamaged(10f);
                GameManager.Instance.TimeManager.TimeWindCostCharge();
            }
            Destroy(gameObject);
        }
    }
}

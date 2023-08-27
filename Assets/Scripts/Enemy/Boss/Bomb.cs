using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Scripting;

public class Bomb : TimeLockObject
{
    [SerializeField] private Collider2D bombCollider2D;
    [SerializeField] private GameObject explosionObject;
    [SerializeField] private Collider2D explosionCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isExploded = false;
    private bool isActiveBomb = false;

    public void Launch(Vector2 dir)
    {
        rigidbody2D.velocity = dir;
        StartCoroutine(ActiveBombCo());
    } 

    private IEnumerator ActiveBombCo()
    {
        yield return new WaitForSeconds(0.5f);
        isActiveBomb = true;
        bombCollider2D.isTrigger = false;
    }

    private void FixedUpdate()
    {
        if (!timeLocked && !isExploded)
        {
            rigidbody2D.velocity += Vector2.down * 0.1f;
        }

        if (isActiveBomb && !isExploded)
        {
            List<Collider2D> overlappedColliders = new();
            bombCollider2D.OverlapCollider(new ContactFilter2D().NoFilter(), overlappedColliders);

            foreach(Collider2D c in overlappedColliders)
            {
                switch(c.gameObject.layer)
                {
                    case 0:
                    case 3:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 15:
                    case 16:
                    case 18:
                    case 20:
                        if (!isExploded && !c.isTrigger)
                        {
                            isExploded = true;
                            StartCoroutine(Explosion());
                            return;
                        }
                        break;
                }
            }
        }

        
    }

    public override void HandleTimeLock()
    {
        if (timeLocked)
        {
            if (nowTimeLockCoroutine != null)
            {
                StopCoroutine(nowTimeLockCoroutine);
                nowTimeLockCoroutine = null;
            }
            GetTimeUnLocked();
        }
        else if (isActiveBomb)
        {
            GetTimeLocked();
        }
    }

    public override void SetSortingLayer(string layer)
    {
        if (isActiveBomb)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = layer;
        }
    }

    private void OnCollisionEnter2D()
    {
        if (!isExploded)
        {
            isExploded = true;
            StartCoroutine(Explosion());
        }
    }

    private IEnumerator Explosion()
    {
        spriteRenderer.enabled = false;
        rigidbody2D.velocity = Vector2.zero;
        bombCollider2D.isTrigger = true;
        if (timeLocked)
        {
            GetTimeUnLocked();
            rigidbody2D.velocity = Vector2.zero;
        }
        explosionObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        List<Collider2D> overlappedColliders = new();
        explosionCollider2D.OverlapCollider(new ContactFilter2D().NoFilter(), overlappedColliders);

        foreach(Collider2D c in overlappedColliders)
        {
            if (c.gameObject.layer == 3 && !c.isTrigger) //Player
            {
                c.GetComponentInParent<Player>().GetDamaged(20f);
            }
            else if (c.gameObject.layer == 20) // Rock
            {
                Destroy(c.gameObject);
            }
            else if (c.gameObject.layer == 7) // Boss
            {
                c.GetComponent<Boss>().GetDamaged(200f);
            }
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}

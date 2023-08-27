using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public enum BossPattern
{
    Wait,
    ThrowBomb,
    ShockWave,
    DropRock,
    Rush,
    Die,
}

public class BossController : MonoBehaviour
{
    private BossPattern nowBossPattern = BossPattern.Wait;
    private Coroutine nowPatternCoroutine;
    [SerializeField] private GameObject shockWavePrefab;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Player player;
    private SpriteRenderer spriteRenderer;
    private Vector2 sightDirection = Vector2.left;
    private bool isRushing = false;
    private float rushSpeed = 5f;
    private float bombSpeed = 5f;
    private new Rigidbody2D rigidbody2D;
    private (float minX, float maxX) mapSizeX = new(-20f, 18f);
    private float rockHeight = 104f;
    private Animator animator;
    private float startPosY;
    private PolygonCollider2D polygonCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }
    
    private void Start()
    {
        startPosY = transform.position.y;
        StartPattern(BossPattern.Wait, 5f);
    }

    private void SeePlayer()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            sightDirection = Vector2.right;
        }
        else
        {
            spriteRenderer.flipX = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
            sightDirection = Vector2.left;
        }
    }

    public void StartPattern(BossPattern pattern, float waitTime = 0f, bool interrupt = false)
    {
        if (pattern == BossPattern.Die)
        {
            StopCoroutine(nowPatternCoroutine);
        }

        if (interrupt)
        {
            StopCoroutine(nowPatternCoroutine);
        }

        nowBossPattern = pattern;
        switch(pattern)
        {
            case BossPattern.Wait:
                nowPatternCoroutine = StartCoroutine(WaitCo(waitTime));
                break;
            case BossPattern.ThrowBomb:
                nowPatternCoroutine = StartCoroutine(ThrowBombCo());
                break;
            case BossPattern.ShockWave:
                nowPatternCoroutine = StartCoroutine(ShockWaveCo());
                break;
            case BossPattern.DropRock:
                nowPatternCoroutine = StartCoroutine(DropRockCo());
                break;
            case BossPattern.Rush:
                nowPatternCoroutine = StartCoroutine(RushCo());
                break;
            case BossPattern.Die:
                nowPatternCoroutine = StartCoroutine(DieCo());
                break;
            default:
                throw new ArgumentOutOfRangeException();

        }
    }

    private IEnumerator WaitCo(float waitTime)
    {
        Debug.Log("Start Wait : " + waitTime.ToString());
        yield return new WaitForSeconds(waitTime);
        SeePlayer();
        StartPattern(RandomAttackPattern());
    }

    private IEnumerator ThrowBombCo()
    {
        animator.SetTrigger("throw");
        transform.position += Vector3.down * 0.15f;
        yield return new WaitForSeconds(0.1f);
        transform.position += Vector3.up * 0.15f;
        float nowPlayerPosX = player.transform.position.x;
        Vector2 dir = new Vector2(bombSpeed * sightDirection.x, Mathf.Abs(nowPlayerPosX - transform.position.x) / bombSpeed * 2.5f);
        GameObject bombObj = Instantiate<GameObject>(bombPrefab, transform.position, Quaternion.identity);
        bombObj.GetComponent<Bomb>().Launch(dir);
        yield return null;
        StartPattern(BossPattern.Wait, 5f);
    }

    private IEnumerator ShockWaveCo()
    {
        animator.SetTrigger("attackReady");
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        GameObject shockWaveObj = Instantiate<GameObject>(shockWavePrefab, transform.position + Vector3.down * 1.25f, Quaternion.identity);
        shockWaveObj.GetComponent<ShockWave>().Launch(sightDirection);
        yield return null;
        StartPattern(BossPattern.Wait, 5f);
    }

    private IEnumerator DropRockCo()
    {
        animator.SetTrigger("attackReady");
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.1f);
        (float x1, float x2) rockPosX = SelectRockPosX();
        GameObject rockObj1 = Instantiate<GameObject>(rockPrefab, new Vector3(rockPosX.x1, rockHeight, 0f), Quaternion.identity);
        rockObj1.GetComponent<Rock>().CountDown();
        GameObject rockObj2 = Instantiate<GameObject>(rockPrefab, new Vector3(rockPosX.x2, rockHeight, 0f), Quaternion.identity);
        rockObj2.GetComponent<Rock>().CountDown();
        yield return null;
        StartPattern(BossPattern.Wait, 10f);
    }

    private (float x1, float x2) SelectRockPosX()
    {
        float x1 = UnityEngine.Random.Range(mapSizeX.minX + 3f, mapSizeX.maxX -3f);
        float x2 = 0f;
        if (x1 > (mapSizeX.minX + mapSizeX.maxX)/2f)
        {
            x2 = UnityEngine.Random.Range(mapSizeX.minX + 1f, x1 - 1f);
        }
        else
        {
            x2 = UnityEngine.Random.Range(x1 + 1f, mapSizeX.maxX -1f);
        }
        return (x1, x2);
    }

    private IEnumerator RushCo()
    {
        animator.SetBool("isRush", true);
        transform.position += Vector3.down * 0.25f;
        yield return new WaitForSeconds(0.1f);
        float nowPlayerPosX = player.transform.position.x;
        bool bigger = (transform.position.x - nowPlayerPosX) > 0f;
        Debug.Log(nowPlayerPosX);
        isRushing = true;
        rigidbody2D.velocity = sightDirection * rushSpeed;

        while (Mathf.Abs(transform.position.x - nowPlayerPosX) > 1f)
        {
            if (bigger)
            {
                if (transform.position.x - nowPlayerPosX <= 0f)
                {
                    break;
                }
            }
            else
            {
                if (transform.position.x - nowPlayerPosX > 0f)
                {
                    break;
                }
            }
            yield return null;
        }
        animator.SetBool("isRush", false);
        transform.position += Vector3.up * 0.25f;
        isRushing = false;
        rigidbody2D.velocity = new Vector2(0f, 0f);
        StartPattern(BossPattern.Wait, 5f);
    }

    private IEnumerator DieCo()
    {
        transform.position += Vector3.down * 0.5f;
        animator.SetTrigger("isDead0");
        animator.SetTrigger("isDead1");
        Debug.Log("Die");
        yield return null;
    }

    private BossPattern RandomAttackPattern()
    {
        System.Random random = new();
        int randomNum = random.Next(1, 5);
        return ((BossPattern) randomNum);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 3 && isRushing)
        {
            player.GetDamaged(40f);
            isRushing = false;
            animator.SetBool("isRush", false);
            transform.position += Vector3.up * 0.25f;
            rigidbody2D.velocity = new Vector2(0f, 0f);
            StartPattern(BossPattern.Wait, 5f, true);
        }
    }
}

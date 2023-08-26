using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : MonoBehaviour, IEnemyInterface
{
    [SerializeField]
    private float maxHP = 20f;
    private float nowHP;
    public float NowHP => nowHP;
    private GameStateManager gameStateManager;
    private Animator animator;
    private TimeZoneThug timeZoneThug;
    private ThugController thugController;
    private SpriteRenderer attackSpriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        timeZoneThug = GetComponent<TimeZoneThug>();
        thugController = GetComponent<ThugController>();
        attackSpriteRenderer = thugController.attackColliderSpriteRenderer;
    }
    private void Start()
    {
        gameStateManager = GameManager.Instance.GameStateManager;
        nowHP = maxHP;
    }

    public void GetDamaged(float damage)
    {
        nowHP -= damage;

        if (nowHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Thug Die!");
        attackSpriteRenderer.enabled = false;
        timeZoneThug.Die(GameManager.Instance.TimeZoneManager.NowTimeZone);
    }
}

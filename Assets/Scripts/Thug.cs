using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : MonoBehaviour
{
    private float maxHP = 20f;
    private float nowHP;
    public float NowHP => nowHP;
    private GameStateManager gameStateManager;
    private Animator animator;
    private TimeZoneThug timeZoneThug;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        timeZoneThug = GetComponent<TimeZoneThug>();
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
        timeZoneThug.Die(GameManager.Instance.TimeZoneManager.NowTimeZone);
    }

}

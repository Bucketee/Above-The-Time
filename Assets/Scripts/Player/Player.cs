using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float maxHP = 10f;
    private float nowHP;
    public float NowHP => nowHP;
    private GameStateManager gameStateManager;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        gameStateManager = GameManager.Instance.GameStateManager;
        nowHP = maxHP; //temp
        // nowHP will be set by DataManger
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
        animator.SetBool("isDead", true);
        gameStateManager.ChangeGameState(GameState.GameOver);
    }
}

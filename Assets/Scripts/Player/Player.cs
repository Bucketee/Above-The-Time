using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float maxHP = 100f;
    private float nowHP;
    public float NowHP => nowHP;
    private GameStateManager gameStateManager;
    private Animator animator;
    [SerializeField] private bool isInvincible = false;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        gameStateManager = GameManager.Instance.GameStateManager;
        nowHP = DataManager.Instance.NowPlayerHP;
    }

    public void GetDamaged(float damage)
    {
        if (!isInvincible)
        {
            nowHP -= damage;
            Debug.Log(nowHP);
            DataManager.Instance.SetPlayerHP(nowHP);
            if (nowHP <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvincibleCo());
            }
        }
    }

    private IEnumerator InvincibleCo()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.1f);
        isInvincible = false;
    }

    public void Die()
    {
        Debug.Log("Die!");
        animator.SetTrigger("isDead0");
        animator.SetTrigger("isDead1");
        Invoke("GameOver", 0.3f);
    }

    public void GameOver()
    {
        gameStateManager.ChangeGameState(GameState.GameOver);
    }
}

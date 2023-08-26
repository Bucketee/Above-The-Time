using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Boss : MonoBehaviour, IEnemyInterface
{
    private float maxHP = 100f;
    private float nowHP;
    public float NowHP => nowHP;

    private BossController bossController;

    private void Start()
    {
        bossController = GetComponent<BossController>();
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

    private void Die()
    {
        Debug.Log("Boss Destroyed");
        bossController.StartPattern(BossPattern.Die);
    }
    
}

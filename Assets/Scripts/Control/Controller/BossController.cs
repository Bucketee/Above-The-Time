using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum BossPattern
{
    Wait,
    ThrowRock,
    LaunchRocket,
    DropRock,
    Rush,
    Die,
}

public class BossController : MonoBehaviour
{
    private BossPattern nowBossPattern = BossPattern.Wait;
    private Coroutine nowPatternCoroutine;
    private float waitTime = 1f;
    
    private void Start()
    {
        StartPattern(BossPattern.Wait);
    }

    public void StartPattern(BossPattern pattern)
    {
        if (pattern == BossPattern.Die)
        {
            StopCoroutine(nowPatternCoroutine);
        }
        nowBossPattern = pattern;
        switch(pattern)
        {
            case BossPattern.Wait:
                nowPatternCoroutine = StartCoroutine(WaitCo());
                break;
            case BossPattern.ThrowRock:
                nowPatternCoroutine = StartCoroutine(ThrowRockCo());
                break;
            case BossPattern.LaunchRocket:
                nowPatternCoroutine = StartCoroutine(LaunchRocketCo());
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

    private IEnumerator WaitCo()
    {
        Debug.Log("Wait");
        yield return new WaitForSeconds(waitTime);
        StartPattern(RandomAttackPattern());
    }

    private IEnumerator ThrowRockCo()
    {
        Debug.Log("Throw Rock");
        yield return null;
        StartPattern(BossPattern.Wait);
    }

    private IEnumerator LaunchRocketCo()
    {
        Debug.Log("Launch Rocket");
        yield return null;
        StartPattern(BossPattern.Wait);
    }

    private IEnumerator DropRockCo()
    {
        Debug.Log("Drop Rock");
        yield return null;
        StartPattern(BossPattern.Wait);
    }

    private IEnumerator RushCo()
    {
        Debug.Log("Rush");
        yield return null;
        StartPattern(BossPattern.Wait);
    }

    private IEnumerator DieCo()
    {
        Debug.Log("Die");
        yield return null;
    }

    private BossPattern RandomAttackPattern()
    {
        System.Random random = new();
        int randomNum = random.Next(1, 5);
        return ((BossPattern) randomNum);
    }
}

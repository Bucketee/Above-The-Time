using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour, IEnemyInterface
{
    private float maxHP = 1000f;
    private float nowHP;
    public float NowHP => nowHP;
    private float targetSliderValue = 1f;
    private float nowSliderValue = 1f;
    [SerializeField] private Slider bossHPSlider;
    private bool sliderChanging = false;
    private bool isDead = false;

    private BossController bossController;

    private void Start()
    {
        bossController = GetComponent<BossController>();
        nowHP = maxHP;
    }

    public void GetDamaged(float damage)
    {
        nowHP -= damage;
        targetSliderValue = nowHP/maxHP;
        if (!sliderChanging)
        {
            StartCoroutine(HPSliderCo());
        }
        if (nowHP <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private IEnumerator HPSliderCo()
    {
        sliderChanging = true;
        while((targetSliderValue - nowSliderValue > 0.001f))
        {
            nowSliderValue = Mathf.Lerp(nowSliderValue, targetSliderValue, Time.deltaTime);
            bossHPSlider.value = nowSliderValue;
            yield return null;
        }
        bossHPSlider.value = targetSliderValue;
        sliderChanging = false;
    }

    private void Die()
    {
        Debug.Log("Boss Destroyed");
        GameManager.Instance.StoryManager.SelectStory(StoryProgress.BackToTree);
        bossController.StartPattern(BossPattern.Die);
    }
    
}

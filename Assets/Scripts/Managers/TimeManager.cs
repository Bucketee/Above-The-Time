using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Time Wind Cost")]
    [SerializeField] private GameObject timeWindCostBar;
    [SerializeField] public float timeWindCost = 100f;
    [SerializeField] public float timeWindCostMax = 100f;
    [SerializeField] public float timeWindCostRegen = 0.1f;
    private float timeWindCostRegenByAttack = 1f;
    private Image costBarImage;

    private TimeLockObject nowTimeLockedObject = null;
    public TimeLockObject NowTimeLockedObject => nowTimeLockedObject;

    private void Awake()
    {
        costBarImage = timeWindCostBar.GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if(timeWindCost < timeWindCostMax) { timeWindCost += timeWindCostRegen; }
        costBarImage.fillAmount = timeWindCost / timeWindCostMax;
    }

    public void TimeWindCostCharge()
    {
        timeWindCost += timeWindCostRegenByAttack;
        timeWindCost = Mathf.Min(timeWindCost, timeWindCostMax);
    }

    public void SetNowTimeLockedObject(TimeLockObject timeLockObject)
    {
        nowTimeLockedObject?.GetTimeUnLocked();
        nowTimeLockedObject = timeLockObject;
    }

    public void UnSetNowTimeLockedObject()
    {
        nowTimeLockedObject = null;
    }
}

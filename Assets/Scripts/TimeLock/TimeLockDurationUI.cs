using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLockDurationUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject timeLockDurationBar;
    [SerializeField] private GameObject timeLockedIcon;
    private TimeManager timeManager;
    private Image barImage;


    private void Awake()
    {
        barImage = timeLockDurationBar.GetComponent<Image>();
    }

    private void Start()
    {
        timeManager = GameManager.Instance.TimeManager;
    }

    private void Update()
    {
        if (timeManager?.NowTimeLockedObject)
        {
            TimeLockDurationUIHandle(timeManager.NowTimeLockedObject.maxDuration, timeManager.NowTimeLockedObject.currentDuration);
        }
        else
        {
            TimeLockDurationUIHandle(0f, 0f);
        }
    }

    private void TimeLockDurationUIHandle(float mDuration, float cDuration)
    {
        if(mDuration > 0)
        {
            timeLockedIcon.SetActive(true);
            barImage.fillAmount = cDuration / mDuration;
        }
        else
        {
            timeLockedIcon.SetActive(false);
            barImage.fillAmount = 0;
        }
    }
    
}

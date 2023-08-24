using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLockDurationUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject timeLockDurationBar;
    [SerializeField] private GameObject timeLockedIcon;
    private Image barImage;


    private void Awake()
    {
        barImage = timeLockDurationBar.GetComponent<Image>();
        TimeLockObject.TimeLockDurationSend += TimeLockDurationUIHandle;
    }

    private void TimeLockDurationUIHandle(float mDuration, float cDuration)
    {
        if(mDuration > 0)
        {
            timeLockedIcon.SetActive(true);
            barImage.fillAmount = (float) cDuration / mDuration;
        }
        else
        {
            timeLockedIcon.SetActive(false);
            barImage.fillAmount = 0;
        }
    }
    
}

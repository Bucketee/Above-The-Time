using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource backgroundAudioSource;

    [Header("Sound Clip")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip rockBreakSound;
    [SerializeField] private AudioClip rockReverseSound;
    [SerializeField] private AudioClip timeLockSound;
    [SerializeField] private AudioClip timeUnlockSound;
    [SerializeField] private AudioClip timeTravelSound;
    [SerializeField] private AudioClip shootingSound;
    private bool nowSound = false;


    [Header("Background Music")]
    [SerializeField] private AudioClip backGroundMusic1;
    [SerializeField] private AudioClip backGroundMusic2;
    private float pausedTime = 0f;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ChangeBGM1();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) { MuteBGM(); }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (backgroundAudioSource.isPlaying) { PauseBGM(); }
            else { PlayBGM(); }
        }
    }

    #region BGM Methods
    public void PlayBGM()
    {
        if(backgroundAudioSource.clip != null)
        {
            backgroundAudioSource.time = pausedTime;
            backgroundAudioSource.Play();
        }
    }
    public void PauseBGM()
    {
        if(backgroundAudioSource.clip != null)
        {
            pausedTime = backgroundAudioSource.time;
            backgroundAudioSource.Pause();
        }
    }
    public void StopBGM()
    {
        if(backgroundAudioSource.clip != null)
        {
            pausedTime = 0f;
            backgroundAudioSource.Stop();
        }
    }
    public void MuteBGM()
    {
        if(backgroundAudioSource.clip != null)
        {
            backgroundAudioSource.volume = 1f - backgroundAudioSource.volume;
        }
    }
    public void ChangeBGM1()
    {
        if (backGroundMusic1 != null && backgroundAudioSource.clip != backGroundMusic1)
        {
            backgroundAudioSource.Stop();
            backgroundAudioSource.clip = backGroundMusic1;
            pausedTime = 0f;
            backgroundAudioSource.Play();
        }
    }
    public void ChangeBGM2()
    {
        if (backGroundMusic2 != null && backgroundAudioSource.clip != backGroundMusic2)
        {
            backgroundAudioSource.Stop();
            backgroundAudioSource.clip = backGroundMusic2;
            pausedTime = 0f;
            backgroundAudioSource.Play();
        }
    }
    #endregion

    #region Sound Methods
    public void JumpSound()
    {
        effectAudioSource.volume = 0.25f;
        if (jumpSound != null) { effectAudioSource.PlayOneShot(jumpSound); }
    }
    public void DashSound()
    {
        effectAudioSource.volume = 0.5f;
        if (dashSound != null) { effectAudioSource.PlayOneShot(dashSound); }
    }
    public void RockBreakSound()
    {
        effectAudioSource.volume = 1f;
        if (rockBreakSound != null) { effectAudioSource.PlayOneShot(rockBreakSound); }
    }
    public void RockReverseSound()
    {
        effectAudioSource.volume = 1f;
        if (rockReverseSound != null) { effectAudioSource.PlayOneShot(rockReverseSound); }
    }
    public void TimeLockSound()
    {
        effectAudioSource.volume = 0.5f;
        if (timeLockSound != null) { effectAudioSource.PlayOneShot(timeLockSound); }
    }
    public void TimeUnlockSound()
    {
        effectAudioSource.volume = 0.3f;
        if (timeUnlockSound != null) { effectAudioSource.PlayOneShot(timeUnlockSound); }
    }
    public void TimeTravelSound()
    {
        if (timeTravelSound != null) { effectAudioSource.PlayOneShot(timeTravelSound); }
    }
    public void ShootingSound()
    {
        effectAudioSource.volume = 0.4f;
        if (shootingSound != null) { effectAudioSource.PlayOneShot(shootingSound); }
    }
    #endregion
}

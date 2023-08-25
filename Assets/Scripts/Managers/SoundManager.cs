using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource backgroundAudioSource;

    [Header("Sound Clip")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip leverSound;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioClip clockTickSound;
    [SerializeField] private AudioClip timeWindSound;
    [SerializeField] private AudioClip timeTravelSound;

    [Header("Background Music")]
    [SerializeField] private AudioClip backGroundMusic1;
    [SerializeField] private AudioClip backGroundMusic2;
    private float pausedTime = 0f;

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
        if (jumpSound != null) { effectAudioSource.PlayOneShot(jumpSound); }
    }
    public void DashSound()
    {
        if (jumpSound != null) { effectAudioSource.PlayOneShot(dashSound); }
    }
    public void LeverSound()
    {
        if (jumpSound != null) { effectAudioSource.PlayOneShot(leverSound); }
    }
    public void DoorSound()
    {
        if (jumpSound != null) { effectAudioSource.PlayOneShot(doorSound); }
    }
    public void ClockTickSound()
    {
        if (jumpSound != null) { effectAudioSource.PlayOneShot(clockTickSound); }
    }
    public void TimeWindSound()
    {
        if (jumpSound != null) { effectAudioSource.PlayOneShot(timeWindSound); }
    }
    public void TimeTravelSound()
    {
        if (jumpSound != null) { effectAudioSource.PlayOneShot(timeTravelSound); }
    }
    #endregion
}

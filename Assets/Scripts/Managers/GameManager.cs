using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameStateManager GameStateManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public TimeManager TimeManager { get; private set; }
    public TimeZoneManager TimeZoneManager { get; private set; }
    public TimeZoneLoading TimeZoneLoading { get; private set; }
    public TalkManager TalkManager { get; private set; }
    public TimeLockDurationUI TimeLockDurationUI { get; private set; }
    public StoryManager StoryManager { get; private set; }
    public SoundManager SoundManager { get; private set; }

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

        GameStateManager = GetComponentInChildren<GameStateManager>();
        UIManager = GetComponentInChildren<UIManager>();
        TimeManager = GetComponentInChildren<TimeManager>();
        TimeZoneManager = GetComponentInChildren<TimeZoneManager>();
        TimeZoneLoading = GetComponentInChildren<TimeZoneLoading>();
        TalkManager = GetComponentInChildren<TalkManager>();
        TimeLockDurationUI = GetComponentInChildren<TimeLockDurationUI>();
        StoryManager = GetComponentInChildren<StoryManager>();
        SoundManager = GetComponentInChildren<SoundManager>();
    }
}

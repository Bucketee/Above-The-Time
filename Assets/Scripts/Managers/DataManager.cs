using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private float nowPlayerHP = 100f;
    public float NowPlayerHP => nowPlayerHP;

    private StoryProgress nowStoryProgress = StoryProgress.Tutorial;
    public StoryProgress NowStoryProgress => nowStoryProgress;

    private TimeZone nowTimeZone = TimeZone.Future;
    public TimeZone NowTimeZone => nowTimeZone;

    private bool[] timeZoneCanMoves;
    public bool[] TimeZoneCanMoves => timeZoneCanMoves;

    public static DataManager Instance { get; private set; }

    private Dictionary<StoryProgress, bool[]> storyTimeZoneCanMoveDict = new()
    {
        {StoryProgress.Tutorial, new bool[3]{true, false, true}},
        {StoryProgress.StartSlum, new bool[3]{true, false, true}},
        {StoryProgress.FirstPastSlum, new bool[3]{true, false, true}},
        {StoryProgress.PlantTree, new bool[3]{true, false, true}},
        {StoryProgress.PresentSlum, new bool[3]{true, false, true}},
        {StoryProgress.SecondPastSlum, new bool[3]{true, false, true}},
        {StoryProgress.ChaseAxe, new bool[3]{true, false, true}},
        {StoryProgress.InTower, new bool[3]{true, false, true}},
        {StoryProgress.TopTower, new bool[3]{true, false, true}},
        {StoryProgress.BackToTree, new bool[3]{true, false, true}},
        {StoryProgress.GoToFuture, new bool[3]{true, false, true}},
        {StoryProgress.End, new bool[3]{true, false, true}},  
    };

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

    public void SetStoryProgress(StoryProgress storyProgress)
    {
        nowStoryProgress = storyProgress;
        timeZoneCanMoves = storyTimeZoneCanMoveDict[nowStoryProgress];
    }

    public void SetPlayerHP(float playerHP)
    {
        nowPlayerHP = playerHP;
    }

    public void SetTimeZone(TimeZone timeZone)
    {
        nowTimeZone = timeZone;
    }
}

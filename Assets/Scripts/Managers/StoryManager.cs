using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoryProgress
{
    Tutorial,
    StartSlum,
    FirstPastSlum,
    PlantTree,
    PresentSlum,
    SecondPastSlum,
    ChaseAxe,
    InTower,
    TopTower,
    BackToTree,
    GoToFuture,
    End,
};

public class StoryManager : MonoBehaviour
{
    public StoryProgress CurrentStory;
    private DataManager dataManager;

    private void Start()
    {
        dataManager = DataManager.Instance;
        CurrentStory = dataManager.NowStoryProgress;
        
    }
    public void NextStory()
    {
        CurrentStory += 1;
        dataManager.SetStoryProgress(CurrentStory);
        GameManager.Instance.TimeZoneManager.UpdateCanMove();
        Debug.Log("Changed story to " + CurrentStory);
    }

    public void SelectStory(StoryProgress storyProgress)
    {
        CurrentStory = storyProgress;
        dataManager.SetStoryProgress(CurrentStory);
        GameManager.Instance.TimeZoneManager.UpdateCanMove();
        Debug.Log("Changed story to " + CurrentStory);
    }
}
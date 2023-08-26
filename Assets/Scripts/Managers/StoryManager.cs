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
    public StoryProgress CurrentStory { get; private set; }
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
        Debug.Log("Changed story to " + CurrentStory);
    }

    public void SelectStory(StoryProgress storyProgress)
    {
        CurrentStory = storyProgress;
        dataManager.SetStoryProgress(CurrentStory);
        Debug.Log("Changed story to " + CurrentStory);
    }
}
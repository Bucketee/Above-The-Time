using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
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

    public StoryProgress CurrentStory { get; private set; }

    public void NextStory()
    {
        CurrentStory += 1;
        Debug.Log("Changed story to " + CurrentStory);
    }

    public void SelectStory(StoryProgress storyProgress)
    {
        CurrentStory = storyProgress;
        Debug.Log("Changed story to " + CurrentStory);
    }
}
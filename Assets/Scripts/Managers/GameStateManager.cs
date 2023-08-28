using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GameState // temp
{
    Playing,
    Pause,
    GameOver,
    Loading,
    Talking,
    TimeChanging
}

public class GameStateManager : MonoBehaviour
{
    private GameState previousGameState;
    [SerializeField] private GameState nowGameState = GameState.Playing; //temp
    public GameState NowGameState => nowGameState;


    private Dictionary<GameState, float> gameStateTimeScaleDict = new()
    {
        {GameState.Playing, 1f},
        {GameState.Pause, 0f},
        {GameState.Loading, 1f},
        {GameState.GameOver, 0f},
        {GameState.Talking, 0f},
        {GameState.TimeChanging, 1f}
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (nowGameState == GameState.Pause)
            {
                RedoGameState();
            }
            else if (nowGameState == GameState.Playing)
            {
                ChangeGameState(GameState.Pause);
            }
        }
    }


    public void ChangeGameState(GameState gameState)
    {
        previousGameState = nowGameState;
        nowGameState = gameState;
        Time.timeScale = gameStateTimeScaleDict[nowGameState];
    }
    public void RedoGameState()
    {
        nowGameState = previousGameState;
        previousGameState = GameState.Playing;
        Time.timeScale = gameStateTimeScaleDict[nowGameState];
    }
    // public void Pause()
    // {
    //     previousGameState = nowGameState;
    //     nowGameState = GameState.Pause;
    //     Time.timeScale = 0f;
    // }

    // public void Resume()
    // {
    //     previousGameState = nowGameState;
    //     nowGameState = GameState.Playing;
    //     Time.timeScale = 1f;
    // }
}

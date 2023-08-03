using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState // temp
{
    Playing,
    Pause,
    GameOver,
}

public class GameStateManager : MonoBehaviour
{
    private GameState nowGameState;
    public GameState NowGameState => nowGameState;

    public void Pause()
    {
        nowGameState = GameState.Pause;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        nowGameState = GameState.Playing;
        Time.timeScale = 1f;
    }
}

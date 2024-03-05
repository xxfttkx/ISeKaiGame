using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameState gameState;

    private void Start()
    {
        SetGameState(GameState.GameEnd);
    }
    private void Update()
    {
        
    }
    public bool InGamePlay()
    {
        return gameState == GameState.GamePlay;
    }
    public bool InGamePause()
    {
        return gameState == GameState.GamePause;
    }

    public bool InGameEnd()
    {
        return gameState == GameState.GameEnd;
    }

    public void SetGameState(GameState gameState)
    {
        if (gameState == GameState.GamePause || gameState == GameState.GameEnd) Time.timeScale = 0;
        else if (gameState == GameState.GamePlay) Time.timeScale = 1;
        this.gameState = gameState;
    }
}

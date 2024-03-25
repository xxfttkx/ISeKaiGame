using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameState gameState;

    private void Start()
    {
        SetGameState(GameState.GameEnd);
        //Application.targetFrameRate = 60;
    }
    private void Update()
    {
        
    }
    private void OnEnable()
    {
        
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.EndLevelEvent += OnEndLevelEvent;
    }
    private void OnDisable()
    {
        
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.EndLevelEvent -= OnEndLevelEvent;
    }
    
    void OnEnterLevelEvent(int _)
    {
        SetGameState(GameState.GamePlay);
    }
    void OnExitLevelEvent(int _)
    {
        SetGameState(GameState.GamePause);
    }
    void OnEndLevelEvent()
    {
        SetGameState(GameState.GameEnd);
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

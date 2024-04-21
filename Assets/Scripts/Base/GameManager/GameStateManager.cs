using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameState gameState;

    private void Start()
    {
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif
        SetGameState(GameState.GameEnd);
    }
    private void Update()
    {
        
    }
    private void OnEnable()
    {
        
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
        EventHandler.EndLevelEvent += OnEndLevelEvent;
        EventHandler.ExitDungeonEvent += OnExitDungeonEvent;
    }
    private void OnDisable()
    {
        
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
        EventHandler.EndLevelEvent -= OnEndLevelEvent;
        EventHandler.ExitDungeonEvent -= OnExitDungeonEvent;
    }

    void OnExitDungeonEvent()
    {
        SetGameState(GameState.GameEnd);
    }


    void OnEnterLevelEvent(int _)
    {
        SetGameState(GameState.GamePlay);
    }
    void OnExitLevelEvent(int _)
    {
        SetGameState(GameState.ExitLevel);
    }
    void OnEndLevelEvent(int _)
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

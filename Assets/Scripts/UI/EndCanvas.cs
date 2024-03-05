using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCanvas : Singleton<EndCanvas>
{
    public GameObject overPanel;
    public GameObject winPanel;
    public Button retryButton;
    public Button backButton;
    public Button returnSelectButton;

    protected override void Awake()
    {
        base.Awake();
        retryButton.onClick.AddListener(Retry);
        backButton.onClick.AddListener(BackToTitle);
        returnSelectButton.onClick.AddListener(ReturnToCharacterSelection);
        overPanel.SetActive(false);
    }
    public void EndGame(int type)
    {
        if(type==0)
        {
            //anim
            overPanel.SetActive(true);
        }
        else winPanel.SetActive(true);
    }

    public void Retry()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        
        int l = LevelManager.Instance.currLevel;
        EventHandler.CallExitLevelEvent(l);
        PlayerManager.Instance.StartGame(SaveLoadManager.Instance.GetLastCharsIndexes());
        LevelManager.Instance.StartLevel(l);
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
        UIManager.Instance.InitPlayerPanel();
    }
    public void BackToTitle()
    {
        EventHandler.CallEnterLevelEvent(-1);
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        StartCanvas.Instance.EnterTitle();
        PoolManager.Instance.ClearPools();
    }
    public void ReturnToCharacterSelection()
    {
        EventHandler.CallExitLevelEvent(-1);
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        StartCanvas.Instance.StartGame();
    }
}

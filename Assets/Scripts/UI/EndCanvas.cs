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
        
        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        LevelManager.Instance.Retry();
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
    }
    public void BackToTitle()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);

        EventHandler.CallEnterLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        StartCanvas.Instance.EnterTitle();
    }
    public void ReturnToCharacterSelection()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);

        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        StartCanvas.Instance.EnterSelect();
    }
}

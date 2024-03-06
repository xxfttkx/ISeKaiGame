using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public DefaultBtn retry;
    public DefaultBtn backToSelect;
    public DefaultBtn backToTitle;
    public DefaultBtn backToGame;
    private void Awake()
    {
        retry.btnClick.AddListener(Retry);
        backToSelect.btnClick.AddListener(BackToSelect);
        backToTitle.btnClick.AddListener(BackToTitle);
        backToGame.btnClick.AddListener(BackToGame);
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
    }
    public void Retry()
    {
        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        LevelManager.Instance.Retry();
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
    }
    public void BackToSelect()
    {
        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        StartCanvas.Instance.EnterSelect();
        this.gameObject.SetActive(false);
    }
    public void BackToTitle()
    {
        EventHandler.CallEnterLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        StartCanvas.Instance.EnterTitle();
        this.gameObject.SetActive(false);
    }
    public void BackToGame()
    {
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
        this.gameObject.SetActive(false);
    }
}

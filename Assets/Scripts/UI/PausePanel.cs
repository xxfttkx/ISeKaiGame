using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Button retry;
    public Button backToSelect;
    public Button backToTitle;
    public Button backToGame;
    private void Awake()
    {
        retry.onClick.AddListener(Retry);
        backToSelect.onClick.AddListener(BackToSelect);
        backToTitle.onClick.AddListener(BackToTitle);
        backToGame.onClick.AddListener(BackToGame);
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            BackToGame();
        }
    }
    void Retry()
    {
        int l = LevelManager.Instance.currLevel;
        EventHandler.CallExitLevelEvent(-1);
        LevelManager.Instance.StartLevel(l);
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
    }
    void BackToSelect()
    {
        EventHandler.CallExitLevelEvent(-1);
        StartCanvas.Instance.StartGame();
        this.gameObject.SetActive(false);
    }
    void BackToTitle()
    {
        EventHandler.CallEnterLevelEvent(-1);
        StartCanvas.Instance.EnterTitle();
        this.gameObject.SetActive(false);
    }
    public void BackToGame()
    {
        GameStateManager.Instance.SetGameState(GameState.GamePlay);
        this.gameObject.SetActive(false);
    }
}

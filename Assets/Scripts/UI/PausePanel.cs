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

        UIManager.Instance.EscOnePanel();
    }
    public void BackToSelect()
    {
        UIManager.Instance.EscOnePanel();
        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        StartCanvas.Instance.EnterSelect();
        
    }
    public void BackToTitle()
    {
        UIManager.Instance.EscOnePanel();
        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        StartCanvas.Instance.EnterTitle();
    }
    public void BackToGame()
    {
        UIManager.Instance.EscOnePanel();
    }
}

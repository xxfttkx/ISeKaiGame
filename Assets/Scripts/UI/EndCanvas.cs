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
    public GameObject saveSucc;
    private Coroutine saveSuccCo;

    protected override void Awake()
    {
        base.Awake();
        retryButton.onClick.AddListener(Retry);
        backButton.onClick.AddListener(BackToTitle);
        returnSelectButton.onClick.AddListener(ReturnToCharacterSelection);
        overPanel.SetActive(false);
        saveSucc.SetActive(false);
    }
    private void OnEnable()
    {
        EventHandler.SaveFinishEvent += OnSaveFinishEvent;
    }
    private void OnDisable()
    {
        EventHandler.SaveFinishEvent -= OnSaveFinishEvent;
    }
    void OnSaveFinishEvent()
    {
        if (saveSuccCo != null)
            StopCoroutine(saveSuccCo);
        saveSuccCo = StartCoroutine(DelayHide());
    }
    IEnumerator DelayHide()
    {
        saveSucc.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        saveSucc.SetActive(false);
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
        LevelManager.Instance.Retry();
    }
    public void BackToTitle()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);

        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        UIManager.Instance.EnterTitle();
    }
    public void ReturnToCharacterSelection()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);

        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallEndLevelEvent();
        UIManager.Instance.EnterSelect();
    }
}

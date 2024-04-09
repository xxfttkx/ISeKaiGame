using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCanvas : MonoBehaviour
{
    public GameObject overPanel;
    public GameObject winPanel;
    public GameObject btnPanel;
    public BtnBase retryButton;
    public BtnBase backButton;
    public BtnBase returnSelectButton;
    public GameObject saveSucc;
    private Coroutine saveSuccCo;

    protected void Awake()
    {
        retryButton.btnClick.AddListener(Retry);
        backButton.btnClick.AddListener(BackToTitle);
        returnSelectButton.btnClick.AddListener(ReturnToCharacterSelection);
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        btnPanel.SetActive(false);
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
        btnPanel.SetActive(true);
    }

    public void Retry()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        btnPanel.SetActive(false);
        EventHandler.CallExitLevelEvent(-1);
        LevelManager.Instance.Retry();
    }
    public void BackToTitle()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        btnPanel.SetActive(false);
        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallExitDungeonEvent();
        UIManager.Instance.EnterTitle();
    }
    public void ReturnToCharacterSelection()
    {
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        btnPanel.SetActive(false);
        EventHandler.CallExitLevelEvent(-1);
        EventHandler.CallExitDungeonEvent();
        EventHandler.CallEnterSelectCanvasEvent();
    }
}

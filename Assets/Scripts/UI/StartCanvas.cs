using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : Singleton<StartCanvas>
{
    public GameObject selectCanvas;
    public GameObject startPanel;
    public GameObject languagePanel;
    public BtnBase btnStart;
    public BtnBase btnQuit;
    public ExtraPanel extraPanel;
    protected override void Awake()
    {
        base.Awake();
        startPanel.SetActive(true);
        btnStart.btnClick.AddListener(EnterSelect);
        btnQuit.btnClick.AddListener(QuitGame);
    }
    public void EnterSelect()
    {
        selectCanvas.SetActive(true);
        SelectCanvas.Instance.InitSelecePanel();
        startPanel.SetActive(false);
    }

    public void EnterTitle()
    {
        startPanel.SetActive(true);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void ShowExtraPanel()
    {
        extraPanel.gameObject.SetActive(true);
        extraPanel.Init();
    }
    public void HideExtraPanel()
    {
        extraPanel.gameObject.SetActive(false);
    }
    public void HideLanguagePanel()
    {
        languagePanel.SetActive(false);
    }
}

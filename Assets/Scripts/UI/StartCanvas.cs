using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCanvas : Singleton<StartCanvas>
{
    public GameObject selectCanvas;
    public GameObject startPanel;
    public LanguagePanel languagePanel;
    public SettingsPanel settingsPanel;
    public BtnBase btnStart;
    public BtnBase btnQuit;
    public ExtraPanel extraPanel;
    public DataPanel dataPanel;
    public RankPanel rankPanel;
    protected override void Awake()
    {
        base.Awake();
        startPanel.SetActive(true);
        btnStart.btnClick.AddListener(EnterSelect);
        btnQuit.btnClick.AddListener(QuitGame);
    }
    public void EnterSelect()
    {
        EventHandler.CallEnterSelectCanvasEvent();
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
    public void ShowLanguagePanel()
    {
        languagePanel.Show();
    }
    public void HideLanguagePanel()
    {
        languagePanel.Hide();
    }
    public void ShowSettingsPanel()
    {
        settingsPanel.Show();
    }
    public void HideSettingsPanel()
    {
        settingsPanel.Hide();
    }
    public void ShowDataPanel()
    {
        dataPanel.Show();
    }
    public void HideDataPanel()
    {
        dataPanel.Hide();
    }
    public void ShowRankPanel()
    {
        rankPanel.Show();
    }
    public void HideRankPanel()
    {
        rankPanel.Hide();
    }

}

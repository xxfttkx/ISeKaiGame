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
    protected override void Awake()
    {
        base.Awake();
        startPanel.SetActive(true);
        btnStart.btnClick.AddListener(EnterSelect);
        btnQuit.btnClick.AddListener(QuitGame);
    }
    public void EnterSelect()
    {
        UIManager.Instance.EnterSelect();
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
        languagePanel.gameObject.SetActive(true);
        languagePanel.Init();
    }
    public void HideLanguagePanel()
    {
        languagePanel.gameObject.SetActive(false);
    }
    public void ShowSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(true);
        settingsPanel.Init();
    }
    public void HideSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(false);
    }
    public void ShowDataPanel()
    {
        dataPanel.gameObject.SetActive(true);
        dataPanel.TryInit();
    }
    public void HideDataPanel()
    {
        dataPanel.gameObject.SetActive(false);
    }

}

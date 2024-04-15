using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingsPanel : BasePanel
{
    public bool bInit = false;
    public SoundSettingsPanel soundSettingsPanel;
    public ImageBtnSelect windowed;
    public ImageBtnSelect runInBackground;
    public TextBtnExpand languageTextBtnExpand;
    public TextBtnExpand frameTextBtnExpand;

    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        soundSettingsPanel.Init();
        if (SaveLoadManager.Instance.GetWindowed())
            windowed.Select();
        else
            windowed.CancelSelect();
        if (SaveLoadManager.Instance.GetRunInBackground())
            runInBackground.Select();
        else
            runInBackground.CancelSelect();
        if (bInit) return;
        bInit = true;
        languageTextBtnExpand.Init(Utils.GetLanguageList(), SaveLoadManager.Instance.GetLanguage());
        frameTextBtnExpand.Init(Utils.GetFrameRateList(), SaveLoadManager.Instance.GetFrameRate());
    }
    public void TryClickWindowed()
    {
        SaveLoadManager.Instance.ChangeWindowed();
        if (SaveLoadManager.Instance.GetWindowed())
            windowed.Select();
        else
            windowed.CancelSelect();
    }
    public void TryClickRunInBackground()
    {
        SaveLoadManager.Instance.ChangeRunInBackground();
        if (SaveLoadManager.Instance.GetRunInBackground())
            runInBackground.Select();
        else
            runInBackground.CancelSelect();
    }
}
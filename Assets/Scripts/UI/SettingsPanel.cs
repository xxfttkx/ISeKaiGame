using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public bool bInit;
    public SoundSettingsPanel soundSettingsPanel;
    public ImageBtnSelect windowed;
    public ImageBtnSelect runInBackground;

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
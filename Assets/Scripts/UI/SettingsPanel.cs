using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public bool bInit;
    public int curr = 0;
    public int max = 3;
    public void Init()
    {
        if (bInit) return;
        bInit = true;
    }
    public void ChangeResolution(int index)
    {
        Screen.SetResolution(1920, 1080, (FullScreenMode)(curr));
        curr++;
        curr = curr > max ? 0 : curr;
    }
}
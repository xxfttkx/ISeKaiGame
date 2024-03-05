using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCanvas : Singleton<SelectCanvas>
{
    public SelectPanel selectPanel;
    private bool bInit = false;
    public void InitSelecePanel()
    {
        if (bInit) return;
        bInit = true;
        SlotPanel.Instance.Init();
        selectPanel.Init();
        
    }
}

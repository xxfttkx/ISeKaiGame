using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageBtn : ImageBtn
{
    private TextMeshProUGUI tmp;

    public override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Init(string s)
    {
        tmp.text = s;
    }
    public void Select()
    {
        selected = true;
        BtnExit();
    }
    public void CancelSelect()
    {
        selected = false;
        BtnExit();
    }
}

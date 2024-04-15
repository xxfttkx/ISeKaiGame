using System;
using TMPro;
using UnityEngine;

public class TextBtnSelect : ImageBtnClick
{
    public TextMeshProUGUI content;
    public Color seletedColor;
    public bool selected = false;
    Action tryCkickAct;
    public override void Awake()
    {
        base.Awake();
        if(content == null)
            content = GetComponentInChildren<TextMeshProUGUI>();
    }
    public override void BtnEnter() { base.BtnEnter(); }
    public override void BtnExit() { base.BtnExit(); slot.color = selected ? seletedColor : exitColor; }
    public void Init(string s, Action act)
    {
        content.text = s;
        tryCkickAct = act;
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
    public override void BtnClick()
    {
        base.BtnClick();
        tryCkickAct?.Invoke();
    }
}

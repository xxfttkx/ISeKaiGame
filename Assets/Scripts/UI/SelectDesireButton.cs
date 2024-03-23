using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDesireButton : BtnBase
{
    public Image mark;
    public Image slot;
    public int index;
    public bool selected;
    public Action<int> clickAction;
    public override void Awake()
    {
        base.Awake();
        CancelSelect();
    }

    public override void BtnEnter()
    {
        mark.enabled = true;
        mark.color = new Color(0f, 0f, 0f, 0.5f);
    }
    public override void BtnExit()
    {
        if(!selected)
            mark.enabled = false;
        mark.color = new Color(0f, 0f, 0f, 1f);
    }
    public override void BtnClick()
    {
        clickAction?.Invoke(index);
    }
    public void InitButton(int index,Action<int> act)
    {
        this.index = index;
        clickAction = act;
    }

    internal void Select()
    {
        mark.enabled = true;
        selected = true;
    }

    internal void CancelSelect()
    {
        mark.enabled = false;
        selected = false;
    }
}

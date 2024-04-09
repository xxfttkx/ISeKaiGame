using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerButton : BtnBase
{
    public Image chara;
    public Image slot;
    public int index;
    public bool selected;
    public override void Awake()
    {
        base.Awake();
        CancelSelect();
    }

    public override void BtnEnter()
    {
        slot.enabled = true;
        slot.color = new Color(1f, 1f, 1f, 0.5f);
        SelectedCharPanel.Instance.SelectChar(index);
    }
    public override void BtnExit()
    {
        if(!selected)
            slot.enabled = false;
        slot.color = new Color(1f, 1f, 1f, 1f);
    }
    public void InitButton(int index, Sprite sp)
    {
        chara.sprite = sp;
        this.index = index;
    }

    internal void Select()
    {
        slot.enabled = true;
        selected = true;
    }

    internal void CancelSelect()
    {
        slot.enabled = false;
        selected = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBtn : BtnBase
{
    public Image chara;
    public Image slot;
    public int index;
    public bool selected;
    Action selectCurr;
    public override void Awake()
    {
        base.Awake();
        CancelSelect();
    }

    public override void BtnEnter()
    {
        slot.color = new Color(1f, 1f, 1f, 0.5f);
    }
    public override void BtnExit()
    {
        if(!selected)
            slot.color = new Color(1f, 1f, 1f, 0f);
        else
            slot.color = new Color(1f, 1f, 1f, 1f);
    }
    public override void BtnClick()
    {
        if(selected)
        {
            CancelSelect();
        }
        else
        {
            selectCurr?.Invoke();
        }
        
        
    }
    public void InitButton(int index, Sprite sp,Action fun)
    {
        chara.sprite = sp;
        this.index = index;
        selectCurr = fun;
    }

    internal void Select()
    {
        slot.color = new Color(1f, 1f, 1f, 1f);
        selected = true;
    }

    internal void CancelSelect()
    {
        slot.color = new Color(1f, 1f, 1f, 0f);
        selected = false;
    }
}

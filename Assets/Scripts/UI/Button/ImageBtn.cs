using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageBtn : BtnBase
{
    public Image slot;
    public Image image;
    public Color enterColor;
    public Color seletedColor;
    public Color unSeletedColor;
    public bool selected = false;
    Action selectAct;
    public int index;
    public override void Awake()
    {
        base.Awake();
        if(slot==null)
            slot = GetComponent<Image>();
    }
    public override void BtnEnter() { slot.color = enterColor; }
    public override void BtnExit() { slot.color = selected ? seletedColor : unSeletedColor; }
    public void Init(int i, Sprite sp, Action act)
    {
        index = i;
        image.sprite = sp;
        selectAct = act;
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
        selectAct?.Invoke();
    }
}

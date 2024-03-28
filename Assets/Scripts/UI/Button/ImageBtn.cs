using UnityEngine;
using UnityEngine.UI;

public class ImageBtn : BtnBase
{
    public Image slot;
    public Color enterColor;
    public Color seletedColor;
    public Color unSeletedColor;
    public bool seleted = false;
    public override void Awake()
    {
        base.Awake();
        slot = GetComponent<Image>();
    }
    public override void BtnEnter() { slot.color = enterColor; }
    public override void BtnExit() { slot.color = seleted ? seletedColor : unSeletedColor; }
}

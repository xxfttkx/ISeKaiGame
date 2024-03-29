using UnityEngine;
using UnityEngine.UI;

public class ImageBtnClick : BtnBase
{
    public Image slot;
    public Color enterColor;
    public Color exitColor;
    public override void Awake()
    {
        base.Awake();
        slot = GetComponent<Image>();
    }
    public override void BtnEnter() { slot.color = enterColor; }
    public override void BtnExit() { slot.color = exitColor; }
}

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
        if (slot == null)
            slot = GetComponent<Image>();
    }
    public override void BtnEnter() { base.BtnEnter(); slot.color = enterColor; }
    public override void BtnExit() { base.BtnExit(); slot.color = exitColor; }
}

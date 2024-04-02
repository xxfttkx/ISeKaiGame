using UnityEngine;
using UnityEngine.UI;

public class ImageBtnSelect : BtnBase
{
    //public Image slotImage;
    public Image selecteImage;
    public bool seleted = false;
    public override void Awake()
    {
        base.Awake();
    }
    public override void BtnEnter() { selecteImage.color = new Color(0f, 0f, 0f, 0.5f); }
    public override void BtnExit() { selecteImage.color = seleted ? new Color(0f, 0f, 0f, 1f) : new Color(0f, 0f, 0f, 0f); }
    public void Select()
    {
        seleted = true;
        BtnExit();
    }
    public void CancelSelect()
    {
        seleted = false;
        BtnExit();
    }
}

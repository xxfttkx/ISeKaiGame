using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultBtn : BtnBase
{
    public Image slot;
    public override void Awake()
    {
        base.Awake();
        slot = GetComponent<Image>();
    }
    public override void BtnEnter() { slot.color = new Color(0.17f, 1f, 0f, 1f); }
    public override void BtnExit() { slot.color = new Color(1f, 1f, 1f, 1f); }
}

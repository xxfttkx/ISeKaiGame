using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageBtn : BtnBase
{
    private Image image;
    private TextMeshProUGUI tmp;
    private int index;

    public override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Init(string s,int i)
    {
        index = i;
        tmp.text = s;
    }
    public void Select()
    {
        image.color = Color.red;
    }
    public void CancelSelect()
    {
        image.color = Color.blue;
    }
}

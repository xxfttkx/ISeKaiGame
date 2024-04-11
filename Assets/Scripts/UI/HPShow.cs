using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPShow : MonoBehaviour
{
    public RectTransform hpRect;
    public int minWidth = 16;
    public int maxWidth = 100;
    public void SetHpVal(float val)
    {
        var width = Mathf.Lerp(minWidth, maxWidth, val);
        hpRect.sizeDelta = new Vector2(width, hpRect.sizeDelta.y);
    }
}

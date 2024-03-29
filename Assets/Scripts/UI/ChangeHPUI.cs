using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeHPUI : MonoBehaviour
{
    private TextMeshProUGUI t;
    private Action releaseFun;
    private void Awake()
    {
        t = GetComponent<TextMeshProUGUI>();
    }
    public void Init(Creature c,int num, Action fun)
    {
        this.transform.position = Camera.main.WorldToScreenPoint(c._pos);
        t.text = $"{num}";
        releaseFun = fun;
        StartCoroutine(DelayRelease(c));
    }
    IEnumerator DelayRelease(Creature c)
    {
        var v3 = this.transform.position;
        Vector2 last = c._pos;
        Vector2 diff;
        for (int i =0;i<10;++i)
        {
            t.alpha = i * 0.1f;
            diff = c._pos - last;
            this.transform.position = new Vector2(v3.x, v3.y + i*3)+ diff;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);
        releaseFun?.Invoke();
    }
}

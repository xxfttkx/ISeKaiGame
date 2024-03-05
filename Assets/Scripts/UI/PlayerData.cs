using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public Image image;
    public GameObject hp;
    private RectTransform hpRect;
    private Material material;
    public GameObject buffParent;
    public GameObject buffPrefab;
    public Dictionary<string, Bufftip> buffNameToBuffTip;
    private int index;
    private void Awake()
    {
        hpRect = hp.GetComponent<RectTransform>();
        material = image.material;
        material = new Material(material);
        image.material = material;
        buffNameToBuffTip = new Dictionary<string, Bufftip>();
        SetFieldTime(0);
    }

    public void SetImage(Sprite sp)
    {
        image.sprite = sp;
        image.SetNativeSize();

    }
    public void SetHP(float val)
    {
        // 16-190
        float width = Mathf.Lerp(16, 190, val);
        hpRect.sizeDelta = new Vector2(width, hpRect.sizeDelta.y);
    }
    public void SetFieldTime(float time)
    {
        // 16-190
        material.SetFloat("_GlitchAmount", time);
    }
    public void SetBuffList(Buff buff)
    {
        Bufftip tip;
        if (buffNameToBuffTip.TryGetValue(buff.buffName, out tip))
        {

        }
        else
        {
            var go = Instantiate(buffPrefab, buffParent.transform);
            tip = go.GetComponent<Bufftip>();
            buffNameToBuffTip.Add(buff.buffName, tip);
        }

        tip.SetBuff(buff);
    }
    public void ShowPlayerSettings()
    {
        UIManager.Instance.ShowPlayerSettingsPanel(index);
    }

    internal void Init(int index, Sprite sprite)
    {
        this.index = index;
        SetImage(sprite);
        SetHP(1);
    }
}

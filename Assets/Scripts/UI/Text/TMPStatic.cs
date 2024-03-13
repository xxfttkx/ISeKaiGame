using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPStatic : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public int index;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        EventHandler.LanguageChange+= OnLanguageChange;
        Init();
    }
    private void OnDisable()
    {
        EventHandler.LanguageChange -= OnLanguageChange;
    }

    void Init()
    {
        tmp.text = SOManager.Instance.GetStringByIndex(index);
    }

    private void OnLanguageChange()
    {
        Init();
    }
}

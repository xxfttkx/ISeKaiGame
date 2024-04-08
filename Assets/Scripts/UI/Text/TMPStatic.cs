using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPStatic : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public int index;
    private int language;
    public TextDataList_SO textDataList_SO;

    private void Awake()
    {
        if (tmp == null)
            tmp = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        EventHandler.LanguageChange += OnLanguageChange;
        EventHandler.LoadFinishEvent += OnLoadFinishEvent;
        Init();
    }
    private void OnDisable()
    {
        EventHandler.LanguageChange -= OnLanguageChange;
        EventHandler.LoadFinishEvent -= OnLoadFinishEvent;
    }

    [ContextMenu("Init")]
    void Init()
    {
        tmp.text = textDataList_SO.GetTextString(index, language);
    }

    private void OnLanguageChange(int l)
    {
        language = l;
        Init();
    }
    void OnLoadFinishEvent()
    {
        language = SaveLoadManager.Instance.GetLanguage();
        Init();
    }
}

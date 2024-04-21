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
        TryUpdateLanguage();
        Init();
    }
    private void OnDisable()
    {
        EventHandler.LanguageChange -= OnLanguageChange;
        EventHandler.LoadFinishEvent -= OnLoadFinishEvent;
    }
    void TryUpdateLanguage()
    {
        language = SaveLoadManager.Instance != null ? SaveLoadManager.Instance.GetLanguage() : 1;
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
        TryUpdateLanguage();
        Init();
    }
}

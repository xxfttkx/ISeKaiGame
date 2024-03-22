using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPStatic : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public int index;
    public int language;
    public TextDataList_SO textDataList_SO;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        EventHandler.LanguageChange+= OnLanguageChange;
        EventHandler.LoadFinishEvent+= OnLoadFinishEvent;
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

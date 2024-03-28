using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LanguagePanel : MonoBehaviour
{
    public GameObject parent;
    public GameObject btnPrefab;
    List<LanguageBtn> btns;
    public bool bInit;
    public void Init()
    {
        if (bInit) return;
        bInit = true;
        int n = (int)Language.Max;
        btns = new List<LanguageBtn>(n);
        for (int i = 0; i < n; ++i)
        {
            string s = Utils.GetLanguageString(i);
            var go = Instantiate(btnPrefab, parent.transform);
            var b = go.GetComponent<LanguageBtn>();
            b.Init(s, i);
            var index = i;
            b.btnClick.AddListener(()=>ChangeLanguage(index));
            btns.Add(b);
        }
        var l = SaveLoadManager.Instance.GetLanguage();
        btns[l].Select();
    }
    public void ChangeLanguage(int index)
    {
        for (int i = 0; i < btns.Count; ++i)
        {
            if (i == index) btns[i].Select();
            else btns[i].CancelSelect();
        }
        SaveLoadManager.Instance.SetLanguage(index);
        EventHandler.CallLanguageChange(index);
    }
}
/*
[CustomEditor(typeof(LanguagePanel))]
public class YourScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LanguagePanel script = (LanguagePanel)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Init"))
        {
            // 在这里执行你想要的代码
            script.Init(); // 例如调用脚本中的方法
        }
    }
}*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExtraPanel : Singleton<ExtraPanel>
{
    public GameObject playerBtnPrefab;
    public GameObject playerBtnParent;
    private List<PlayerBtnInExtraPanel> playerBtns;
    public TextMeshProUGUI t;
    private int selected;
    private bool bInit;

    public void Init()
    {
        if (bInit) return;
        bInit = true;
        playerBtns = new List<PlayerBtnInExtraPanel>();
        for (int i = 0; i < SOManager.Instance.characterDataList_SO.characters.Length; ++i)
        {
            var go = Instantiate(playerBtnPrefab, playerBtnParent.transform);
            var btn = go.GetComponent<PlayerBtnInExtraPanel>();
            playerBtns.Add(btn);
            
            var index = SOManager.Instance.characterDataList_SO.characters[i].index;
            var sp = SOManager.Instance.characterDataList_SO.characters[i].sprite;
            btn.InitButton(index, sp);
        }
        Select(0);
    }
    public void Select(int index)
    {
        for (int i = 0; i < playerBtns.Count; ++i)
        {
            if (i == index) playerBtns[i].Select();
            else playerBtns[i].CancelSelect();
        }
        selected = index;
        ShowPlayerExtra();
    }
    public void ShowPlayerExtra()
    {
        var ch = SOManager.Instance.GetPlayerDataByIndex(selected);
        t.text = ch.index+":\n"+ch.desc;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

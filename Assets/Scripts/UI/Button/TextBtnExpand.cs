using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBtnExpand : ImageBtnClick
{
    public GameObject expandParent;
    public GameObject expandPrefab;
    private bool bInit = false;
    private List<TextBtnSelect> textBtnSelectList = new List<TextBtnSelect>();
    private List<string> stringList;
    public int saveLoadIndex;
    private int currSelectIndex = -1;
    public override void Awake()
    {
        base.Awake();
    }
    public override void BtnEnter() { slot.color = enterColor; }
    public override void BtnExit() { slot.color = exitColor; }
    public override void BtnClick()
    {
        base.BtnClick();
        expandParent.SetActive(!expandParent.activeSelf);
    }
    public void Init(List<string> stringList, int currIndex)
    {
        if(stringList==null|| stringList.Count==0)
        {
            Debug.Log("");
            return;
        }
        if (bInit) return;
        bInit = true;
        this.stringList = stringList;
        for (int i = 0; i < stringList.Count; ++i)
        {
            var go = Instantiate(expandPrefab, expandParent.transform);
            go.SetActive(true);
            var tbs = go.GetComponent<TextBtnSelect>();
            var index = i;
            tbs.Init(stringList[i], ()=>TryClickText(index));
            textBtnSelectList.Add(tbs);
        }
        currSelectIndex = currIndex;
        textBtnSelectList[currSelectIndex].Select();
        expandParent.SetActive(false);
    }
    public void TryClickText(int index)
    {
        if(currSelectIndex!=index)
        {
            if(currSelectIndex!=-1)
                textBtnSelectList[currSelectIndex].CancelSelect();
            currSelectIndex = index;
            textBtnSelectList[index].Select();
        }
        EventHandler.CallTextBtnExpandSelectEvent(saveLoadIndex, currSelectIndex);
    }
}

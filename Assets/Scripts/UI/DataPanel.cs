using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DataPanel : BasePanel
{
    public bool bInit = false;
    public GameObject dataRawParent;
    public GameObject dataRawPrefab;
    List<DataRaw> dataRawList = new List<DataRaw>();

    private void OnEnable()
    {
        TryInit();
    }
    public void TryInit()
    {
        InitPlayerData();
    }
    public void InitPlayerData()
    {
         
        if(!bInit)
        {
            bInit = true;
            var characters = SOManager.Instance.characterDataList_SO.characters;
            for (int i = 0; i < characters.Length; ++i)
            {
                if (!characters[i].finished) continue;
                var go = Instantiate(dataRawPrefab, dataRawParent.transform);
                var dataRow = go.GetComponent<DataRaw>();
                dataRow.Init(i);
                dataRawList.Add(dataRow);
            }
        }
        else
        {
            for (int i = 0; i < dataRawList.Count; ++i)
            {
                var dataRow = dataRawList[i];
                dataRow.ReInit();
            }
        }
        
        
    }
}
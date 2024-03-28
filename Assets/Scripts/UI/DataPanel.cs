using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DataPanel : MonoBehaviour
{
    public bool bInit;
    public GameObject dataRawParent;
    public GameObject dataRawPrefab;
    
    public void TryInit()
    {
        if (bInit) return;
        bInit = true;
        InitPlayerData();
    }
    public void InitPlayerData()
    {
        var characters = SOManager.Instance.characterDataList_SO.characters;
        for(int i = 0;i< characters.Length;++i)
        {
            if (!characters[i].finished) continue;
            var go = Instantiate(dataRawPrefab, dataRawParent.transform);
            var dataRow = go.GetComponent<DataRaw>();
            dataRow.Init(i);
            
        }
        
    }
}
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RankPanel : MonoBehaviour
{
    public bool bInit;
    public GameObject rankParent;
    public GameObject rankPrefab;

    private void OnEnable()
    {
        ShowRank();
    }
    public void ShowRank()
    {
        var d = SaveLoadManager.Instance.CharsToLevel;
        if (d != null)
        {
            List<CharsPassLevel> l = new List<CharsPassLevel>();
            foreach (var (k, v) in d)
            {
                var list = Utils.GetListByString(k);
                CharsPassLevel c = new CharsPassLevel(list, v);
                l.Add(c);
            }
            l.Sort();
            foreach (var c in l)
            {

            }
        }
        
    }
}
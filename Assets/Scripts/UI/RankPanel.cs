using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : BasePanel
{
    public bool bInit;
    public GameObject rankParent;
    public GameObject rankPrefab;
    List<RankData> rankDataList = new List<RankData>();

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
            for (int i = 0; i < l.Count; ++i)
            {
                RankData data;
                if (i < rankDataList.Count)
                    data = rankDataList[i];
                else
                {
                    var go = Instantiate(rankPrefab, rankParent.transform);
                    data = go.GetComponent<RankData>();
                    rankDataList.Add(data);
                }
                data.Init(l[i].Indexes, i + 1, l[i].Level);
                data.gameObject.SetActive(true);
            }
            for(int i = l.Count;i< rankDataList.Count;i++)
            {
                rankDataList[i].gameObject.SetActive(false);
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : BasePanel
{
    public bool bInit;
    public GameObject rankParent;
    public GameObject rankPrefab;
    List<CharsPassLevel> charsPassLevelList = new List<CharsPassLevel>();
    List<RankData> rankDataList = new List<RankData>();
    int currPage; // 0-self 1-friend 2-world

    private void OnEnable()
    {
        EventHandler.GetLeaderboardSuccEvent += OnGetLeaderboardSuccEvent;
        ShowLocalRank();
    }
    private void OnDisable()
    {
        EventHandler.GetLeaderboardSuccEvent -= OnGetLeaderboardSuccEvent;
    }
    public void ShowLocalRank()
    {
        currPage = 0;
        var d = SaveLoadManager.Instance.CharsToLevel;
        if (d != null)
        {
            charsPassLevelList.Clear();
            foreach (var (k, v) in d)
            {
                var list = Utils.GetListByString(k);
                CharsPassLevel c = new CharsPassLevel(list, v);
                charsPassLevelList.Add(c);
            }
            charsPassLevelList.Sort();
            for (int i = 0; i < charsPassLevelList.Count; ++i)
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
                data.Init(charsPassLevelList[i].Indexes, i + 1, charsPassLevelList[i].Level);
                data.gameObject.SetActive(true);
            }
            for(int i = charsPassLevelList.Count;i< rankDataList.Count;i++)
            {
                rankDataList[i].gameObject.SetActive(false);
            }
        }

    }
    public void UploadLocalScore()
    {
        if (charsPassLevelList == null || charsPassLevelList.Count == 0)
            return;
        int score = Utils.GetScoreByPlyaerIndexesAndLevel(charsPassLevelList[0].Indexes, charsPassLevelList[0].Level);
        EventHandler.CallTryUploadLocalScoreEvent(score, charsPassLevelList[0].Indexes);
    }
    public void ClickSelfBtn()
    {
        if (currPage != 0)
        {
            ShowLocalRank();
        }
    }
    public void ClickFriendBtn()
    {
        if(currPage!=1)
        {
            currPage = 1;
            EventHandler.CallTryGetFriendLeaderboardEvent();
        }
        
    }
    public void ClickWorldBtn()
    {
        if (currPage != 2)
        {
            currPage = 2;
            EventHandler.CallTryGetWorldLeaderboardEvent();
        }
    }
    void OnGetLeaderboardSuccEvent(List<LeaderboardData> list)
    {
        for (int i = 0; i < list.Count; ++i)
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
            data.Init(list[i].Indexes, list[i].Rank, charsPassLevelList[i].Level, list[i].Name);
            data.gameObject.SetActive(true);
        }
        for (int i = list.Count; i < rankDataList.Count; i++)
        {
            rankDataList[i].gameObject.SetActive(false);
        }
    }
}
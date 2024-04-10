using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamLeaderboard : MonoBehaviour
{
    protected CallResult<LeaderboardFindResult_t> m_LeaderboardFindResult;
    protected CallResult<LeaderboardScoreUploaded_t> m_LeaderboardScoreUploaded;
    protected CallResult<LeaderboardScoresDownloaded_t> m_LeaderboardScoresDownloaded;
    private CGameID m_GameID;
    SteamLeaderboard_t m_steamLeaderboard;
    public int maxScore = 0;
    int[] maxScoreIndexes;
    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameID = new CGameID(SteamUtils.GetAppID());
            m_LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
            m_LeaderboardScoreUploaded = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaerboardScoreUploaded);
            m_LeaderboardScoresDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
            if (m_steamLeaderboard.m_SteamLeaderboard == 0)
            {
                var p = SteamUserStats.FindOrCreateLeaderboard("World", ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
                m_LeaderboardFindResult.Set(p);
            }
        }
        EventHandler.TryUploadLocalScoreEvent += OnTryUploadLocalScoreEvent;
        EventHandler.TryGetFriendLeaderboardEvent += OnTryGetFriendLeaderboardEvent;
        EventHandler.TryGetWorldLeaderboardEvent += OnTryGetWorldLeaderboardEvent;
    }
    private void OnDisable()
    {
        EventHandler.TryUploadLocalScoreEvent -= OnTryUploadLocalScoreEvent;
        EventHandler.TryGetFriendLeaderboardEvent -= OnTryGetFriendLeaderboardEvent;
        EventHandler.TryGetWorldLeaderboardEvent -= OnTryGetWorldLeaderboardEvent;
    }
    void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure)
    {
        if (!SteamManager.Initialized)
            return;
        if (pCallback.m_bLeaderboardFound == 1)
        {
            Debug.Log("m_bLeaderboardFound TRUE");
            m_steamLeaderboard = pCallback.m_hSteamLeaderboard;
        }
        else
        {
            Debug.Log("m_bLeaderboardFound FALSE");
            var p = SteamUserStats.FindOrCreateLeaderboard("World", ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
            m_LeaderboardFindResult.Set(p);
        }
    }


    private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_hSteamLeaderboard == m_steamLeaderboard)
        {
            int n = pCallback.m_cEntryCount;
            if (n > 0)
            {
                Debug.Log("���а񔵓�����" + n);
                int maxDetailsCount = Settings.maxCompanionNum;
                List<LeaderboardData> leaderboardDataList = new List<LeaderboardData>(n);
                for (int i = 0; i < n; i++)
                {
                    LeaderboardEntry_t leaderboardEntry;
                    int[] details = new int[maxDetailsCount];
                    SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out leaderboardEntry, details, maxDetailsCount);
                    var name = SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser);
                    Debug.LogError("�Ñ�name��" + name + "�Ñ��֔�" + leaderboardEntry.m_nScore + "�Ñ�����" + leaderboardEntry.m_nGlobalRank + "Details" + leaderboardEntry.m_cDetails + "  " + SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser));
                    List<int> playerIndexes = GetValidPlayreIndexes(details, leaderboardEntry.m_cDetails);
                    var ld = new LeaderboardData(playerIndexes, leaderboardEntry.m_nScore, name, leaderboardEntry.m_nGlobalRank);
                    leaderboardDataList.Add(ld);
                }
                EventHandler.CallGetLeaderboardSuccEvent(leaderboardDataList);
            }
            else
            {
                Debug.Log("���а񔵓����գ�");
            }
        }
    }

    public void DownloadLeaderboardEntries(ELeaderboardDataRequest req, int negi, int posi)
    {
        Debug.Log("Ո�����а񔵓�" + req.ToString());
        var handle = SteamUserStats.DownloadLeaderboardEntries(m_steamLeaderboard, req, negi, posi);
        m_LeaderboardScoresDownloaded.Set(handle);
    }

    void OnTryUploadLocalScoreEvent(int score, List<int> playerIndexes)
    {
        if (SteamManager.Initialized && m_steamLeaderboard.m_SteamLeaderboard != 0)
        {
            maxScore = score;
            maxScoreIndexes = Utils.GetArrayByList(playerIndexes);
            UploadScore();
        }
    }
    public void UploadScore()
    {
        Debug.Log("�ς��֔�:   " + maxScore);
        var handle = SteamUserStats.UploadLeaderboardScore(m_steamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, maxScore, maxScoreIndexes, maxScoreIndexes.Length);
        m_LeaderboardScoreUploaded.Set(handle);
    }
    private void OnLeaerboardScoreUploaded(LeaderboardScoreUploaded_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_bSuccess != 1)
        {
            UploadScore();
        }
        else
        {
            Debug.Log("�ɹ��ς��rֵ������" + pCallback.m_nScore + "��Ȕ����Ƿ���Ҫ׃����" + pCallback.m_bScoreChanged
                + "�µ�������" + pCallback.m_nGlobalRankNew + "�ϴ�������" + pCallback.m_nGlobalRankPrevious);
            if (pCallback.m_nGlobalRankPrevious == 0 || pCallback.m_bScoreChanged != 0)
            {
                m_steamLeaderboard = pCallback.m_hSteamLeaderboard;
            }
            // DownloadLeaderboardEntries();
            EventHandler.CallUploadLocalScoreSuccEvent();
        }
    }
    void OnTryGetFriendLeaderboardEvent()
    {
        DownloadLeaderboardEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -100, 100);
    }
    void OnTryGetWorldLeaderboardEvent()
    {
        DownloadLeaderboardEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 0, 0);
    }
    List<int> GetValidPlayreIndexes(int[] indexes, int validCount)
    {
        List<int> res = new List<int>(validCount);
        if (indexes.Length < validCount)
        {
            Debug.Log("why cheat..");
        }
        else
        {
            for (int i = 0; i < validCount; ++i)
            {
                res.Add(indexes[i]);
            }
        }
        return res;
    }
}

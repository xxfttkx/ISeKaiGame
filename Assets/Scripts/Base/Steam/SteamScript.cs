using UnityEngine;
using System.Collections;
using Steamworks;
using System.Collections.Generic;
using System;

public class SteamScript : MonoBehaviour
{
    public enum Achievement : int
    {
        ACH_Pass10_0,
        ACH_Pass10_1,
        ACH_Pass10_2,
        ACH_Pass10_3,
        ACH_Pass10_4,
        ACH_Pass10_5,
        ACH_Pass10_6,
        ACH_Pass10_7,
        ACH_Pass10_8,
        ACH_Pass10_9,
        ACH_Pass10_10,
        ACH_Pass10_11,
        ACH_Pass10_12,
        ACH_Pass10_13,
        ACH_Pass10_14,
        ACH_Pass10_15,
        ACH_Pass10_16,
        ACH_Pass10_17,
        ACH_Pass10_18,
        ACH_Pass10_19,
        ACH_Pass10_20,
    };
    //TODO achievement¶à´Î´¥·¢

    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserStatsStored_t> m_UserStatsStored;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;
    private CGameID m_GameID;
    // Should we store stats this frame?
    private bool m_bStoreStats;
    // Did we get the stats from Steam?
    private bool m_bRequestedStats;
    private bool m_bStatsValid;
    void Start()
    {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
        }
    }
    private void Update()
    {
        if (!SteamManager.Initialized)
            return;

        if (!m_bRequestedStats)
        {
            // Is Steam Loaded? if no, can't get stats, done
            if (!SteamManager.Initialized)
            {
                m_bRequestedStats = true;
                return;
            }

            // If yes, request our stats
            bool bSuccess = SteamUserStats.RequestCurrentStats();

            // This function should only return false if we weren't logged in, and we already checked that.
            // But handle it being false again anyway, just ask again later.
            m_bRequestedStats = bSuccess;
        }

        if (!m_bStatsValid)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnlockAchievement(Achievement.ACH_Pass10_0);
        }
        // Get info from sources

        // Evaluate achievements
        // TODO Callback To Unlock Achievements

        //Store stats in the Steam database if necessary
        if (m_bStoreStats)
        {
            // already set any achievements in UnlockAchievement

            bool bSuccess = SteamUserStats.StoreStats();
            // If this failed, we never sent anything to the server, try
            // again later.
            m_bStoreStats = !bSuccess;
        }
    }


    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameID = new CGameID(SteamUtils.GetAppID());
            m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
            m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
        }
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
    }
    private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
    {
        if (pCallback.m_bSuccess != 1 || bIOFailure)
        {
            Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
        }
        else
        {
            Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
        }
    }
    public void OnExitLevelEvent(int l)
    {
        if (SteamManager.Initialized)
        {
            var list = PlayerManager.Instance.TrueIndexes;
            var leaderboard = SteamUserStats.FindOrCreateLeaderboard("Total", ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
            if (!(l == Settings.levelMaxNum))
            {
                foreach (var i in list)
                {
                    UnlockAchievement(Achievement.ACH_Pass10_0 + i);
                }

            }
        }
    }

    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        // we may get callbacks for other games' stats arriving, ignore them
        if (!SteamManager.Initialized || (ulong)m_GameID != pCallback.m_nGameID)
            return;
        if (EResult.k_EResultOK == pCallback.m_eResult)
        {
            Debug.Log("Received stats and achievements from Steam\n");

        }
        else
        {
            Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
        }
        m_bStatsValid = true;
    }
    //-----------------------------------------------------------------------------
    // Purpose: Our stats data was stored!
    //-----------------------------------------------------------------------------
    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("StoreStats - success");
            }
            else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
            {
                // One or more stats we set broke a constraint. They've been reverted,
                // and we should re-iterate the values now to keep in sync.
                Debug.Log("StoreStats - some failed to validate");
                // Fake up a callback here so that we re-load the values.
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)m_GameID;
                OnUserStatsReceived(callback);
            }
            else
            {
                Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: An achievement was stored
    //-----------------------------------------------------------------------------
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        // We may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (0 == pCallback.m_nMaxProgress)
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
            }
        }
    }
    public void UnlockAchievement(Achievement achievement)
    {
        if (!SteamManager.Initialized)
            return;
        /*SteamUserStats.SetAchievement(achievement.ToString());
        m_bStoreStats = true;*/
        StartCoroutine(TryDoUntilTrue(() => SteamUserStats.SetAchievement(achievement.ToString())));
    }
    IEnumerator TryDoUntilTrue(Func<bool> a)
    {
        if (a == null) yield break;
        while (true)
        {
            if (a.Invoke())
            {
                m_bStoreStats = true;
                break;
            }
            else
            {
                Debug.Log("Func return false");
                break;
            }
        }
    }
}
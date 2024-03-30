using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Threading.Tasks;

public class SteamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            SteamClient.Init(2909391, true);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
        Steamworks.SteamUserStats.OnAchievementProgress += AchievementChanged;
    }
    private void OnDisable()
    {
        Steamworks.SteamClient.Shutdown();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task GetAllLeaderboardAsync()
    {

        var leaderboard = await SteamUserStats.FindOrCreateLeaderboardAsync("MyLeaderboard",
                                                                           LeaderboardSort.Ascending,
                                                                            LeaderboardDisplay.Numeric);
        Leaderboard l = new Leaderboard();
        var list = await l.GetScoresFromFriendsAsync();
    }
    private void AchievementChanged(Steamworks.Data.Achievement ach, int currentProgress, int progress)
    {
        if (ach.State)
        {
            Debug.Log($"{ach.Name} WAS UNLOCKED!");
        }
    }
}

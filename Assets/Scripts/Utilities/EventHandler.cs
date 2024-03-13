using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action<GameObject, Vector3> CreateEnemyEvent;
    public static void CallCreateEnemyEvent(GameObject enemy, Vector3 pos)
    {
        CreateEnemyEvent?.Invoke(enemy, pos);
    }
    public static event Action SwitchBackgroundEvent;
    public static void CallSwitchBackgroundEvent()
    {
        SwitchBackgroundEvent?.Invoke();
    }
    //
    public static event Action WinCurrLevelEvent;
    public static void CallWinCurrLevelEvent()
    {
        WinCurrLevelEvent?.Invoke();
    }

    // 创造羽毛 索敌 生成位置
    public static event Action<GameObject, Vector3> CreateFeatherEvent;
    public static void CallCreateFeatherEvent(GameObject enemy, Vector3 pos)
    {
        CreateFeatherEvent?.Invoke(enemy, pos);
    }
    // 创造光球 索敌 生成位置
    public static event Action<GameObject, Vector3> CreateGuangQiuEvent;
    public static void CallCreateGuangQiuEvent(GameObject enemy, Vector3 pos)
    {
        CreateGuangQiuEvent?.Invoke(enemy, pos);
    }

    // 创造泡泡 索敌 生成位置
    public static event Action<GameObject, Vector3> CreateBubbleEvent;
    public static void CallCreateBubbleEvent(GameObject enemy, Vector3 pos)
    {
        CreateBubbleEvent?.Invoke(enemy, pos);
    }

    public static event Action<Vector3, Vector2> CreateBulletEvent;
    public static void CallCreateBulletEvent(Vector3 pos, Vector2 force)
    {
        CreateBulletEvent?.Invoke(pos, force);
    }

    public static event Action<GameObject> ReleaseBulletEvent;
    public static void CallReleaseBulletEvent(GameObject obj)
    {
        ReleaseBulletEvent?.Invoke(obj);
    }

    public static event Action<Vector3> CreateDeadEnemyEvent;
    public static void CallCreateDeadEnemyEvent(Vector3 pos)
    {
        CreateDeadEnemyEvent?.Invoke(pos);
    }

    public static event Action StartNewGameEvent;
    public static void CallStartNewGameEvent()
    {
        StartNewGameEvent?.Invoke();
    }

    public static event Action EndLevelEvent;
    public static void CallEndLevelEvent()
    {
        EndLevelEvent?.Invoke();
    }

    public static event Action RestartThisLevelEvent;
    public static void CallRestartThisLevelEvent()
    {
        RestartThisLevelEvent?.Invoke();
    }


    public static event Action UpdateRecordPanelUIEvent;
    public static void CallUpdateRecordPanelUIEvent()
    {
        UpdateRecordPanelUIEvent?.Invoke();
    }
    public static event Action<Vector3, int> HurtPlayerEvent;
    public static void CallHurtPlayerEvent(Vector3 enemyPos, int damage)
    {
        HurtPlayerEvent?.Invoke(enemyPos, damage);
    }

    /// <summary>
    /// 爆炸特效
    /// </summary>
    public static event Action<Vector3> CreateBoomEvent;
    public static void CallCreateBoomEvent(Vector3 pos)
    {
        CreateBoomEvent?.Invoke(pos);
    }

    /// <summary>
    /// 枪口特效
    /// </summary>
    public static event Action<Vector3> CreateGunFireEvent;
    public static void CallCreateGunFireEvent(Vector3 pos)
    {
        CreateGunFireEvent?.Invoke(pos);
    }



    /// <summary>
    /// 切换场景
    /// </summary>
    public static event Action<int> TransitionEvent;
    public static void CallTransitionEvent(int level)
    {
        TransitionEvent?.Invoke(level);
    }

    /// <summary>
    /// 更新游戏状态
    /// </summary>
    public static event Action<GameState> UpdateGameStateEvent;
    public static void CallUpdateGameStateEvent(GameState gameState)
    {
        UpdateGameStateEvent?.Invoke(gameState);
    }

    /// <summary>
    /// 游戏场景切换前
    /// </summary>
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    /// <summary>
    /// 游戏场景切换后
    /// </summary>
    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }





    //看看用哪个
    /// <summary>
    /// 声音特效
    /// </summary>
    public static event Action<SoundDetails> CreateSoundEvent;
    public static void CallCreateSoundEvent(SoundDetails soundDetails)
    {
        CreateSoundEvent?.Invoke(soundDetails);
    }

    //音效
    public static event Action<SoundDetails> InitSoundEffect;
    public static void CallInitSoundEffect(SoundDetails soundDetails)
    {
        InitSoundEffect?.Invoke(soundDetails);
    }

    public static event Action<SoundName> PlaySoundEvent;
    public static void CallPlaySoundEvent(SoundName soundName)
    {
        PlaySoundEvent?.Invoke(soundName);
    }


    /// <summary>
    /// PlayerDeadEvent
    /// </summary>
    public static event Action<int> PlayerDeadEvent;
    public static void CallPlayerDeadEvent(int playerIndex)
    {
        PlayerDeadEvent?.Invoke(playerIndex);
    }


    /// <summary>
    /// PlayerDeadEvent
    /// </summary>
    public static event Action<int> ExitLevelEvent;
    public static void CallExitLevelEvent(int level)
    {
        ExitLevelEvent?.Invoke(level);
    }

    /// <summary>
    /// PlayerDeadEvent
    /// </summary>
    public static event Action<int> EnterLevelEvent;
    public static void CallEnterLevelEvent(int level)
    {
        EnterLevelEvent?.Invoke(level);
    }

    /// <summary>
    /// ChangePlayerOnTheField
    /// </summary>
    public static event Action<Player> ChangePlayerOnTheFieldEvent;
    public static void CallChangePlayerOnTheFieldEvent(Player p)
    {
        ChangePlayerOnTheFieldEvent?.Invoke(p);
    }

    /// <summary>
    /// PlayerKillEnemyEvent
    /// </summary>
    public static event Action<int> PlayerKillEnemyEvent;
    public static void CallPlayerKillEnemyEvent(int playerIndex)
    {
        PlayerKillEnemyEvent?.Invoke(playerIndex);
    }

    /// <summary>
    /// 读档结束
    /// </summary>
    public static event Action LoadFinishEvent;
    public static void CallLoadFinishEvent()
    {
        LoadFinishEvent?.Invoke();
    }
    /// <summary>
    /// 存档结束
    /// </summary>
    public static event Action SaveFinishEvent;
    public static void CallSaveFinishEvent()
    {
        SaveFinishEvent?.Invoke();
    }

    /// <summary>
    /// extra chaange
    /// </summary>
    public static event Action<int,int,int> ExtraChangeEvent;
    public static void CallExtraChangeEvent(int playerIndex,int extraIndex, int selectedIndex)
    {
        ExtraChangeEvent?.Invoke(playerIndex, extraIndex, selectedIndex);
    }

    public static event Action LanguageChange;
    public static void CallLanguageChange()
    {
        LanguageChange?.Invoke();
    }
}
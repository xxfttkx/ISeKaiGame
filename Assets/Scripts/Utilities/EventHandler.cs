using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{

    public static event Action SwitchBackgroundEvent;
    public static void CallSwitchBackgroundEvent()
    {
        SwitchBackgroundEvent?.Invoke();
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
    public static event Action<int,int,int> DesireChangeEvent;
    public static void CallDesireChangeEvent(int playerIndex,int extraIndex, int selectedIndex)
    {
        DesireChangeEvent?.Invoke(playerIndex, extraIndex, selectedIndex);
    }

    public static event Action<int> LanguageChange;
    public static void CallLanguageChange(int l)
    {
        LanguageChange?.Invoke(l);
    }

    public static event Action<int,float> PlayerHpValChangeEvent;
    public static void CallPlayerHpValChangeEvent(int index,float val)
    {
        if (index < 0) return;
        PlayerHpValChangeEvent?.Invoke(index,val);
    }
    public static event Action<int,Buff> BuffChangeEvent;
    public static void CallBuffChangeEvent(int index, Buff b)
    {
        if (index < 0) return;
        BuffChangeEvent?.Invoke(index,b);
    }
    public static event Action<int, Buff> BuffRemoveEvent;
    public static void CallBuffRemoveEvent(int index, Buff b)
    {
        if (index < 0) return;
        BuffRemoveEvent?.Invoke(index, b);
    }


    public static event Action<int, int,int> PlayerHurtPlayerEvent;
    public static void CallPlayerHurtPlayerEvent(int atkIndex, int hurtIndex, int atk)
    {
        PlayerHurtPlayerEvent?.Invoke(atkIndex, hurtIndex,atk);
    }

    public static event Action<EnemyBase, int,int> EnemyHurtPlayerEvent;
    public static void CallEnemyHurtPlayerEvent(EnemyBase e, int hurtIndex, int atk)
    {
        EnemyHurtPlayerEvent?.Invoke(e, hurtIndex,atk);
    }
}
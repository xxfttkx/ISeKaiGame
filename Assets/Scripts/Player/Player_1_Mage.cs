using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1_Mage : Player_Single
{
    protected override void Awake()
    {
        character.index = 1;
        base.Awake();
    }
    public override void Reset()
    {
        base.Reset();
    }
    protected override void OnEnable()
    {
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
        base.OnDisable();
    }

    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        PoolManager.Instance.CreateFeather(e, this.transform.position);
        yield break;
    }
    public override void AddBuffBeforeStart()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            p.ApplyBuff("player1", -1, GetBuffBonus(), 0f, 0f, 0f, 0f, ApplyBuffType.Override);
        }
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && extraIndex == 2)
        {
            AddBuffBeforeStart();
            EventHandler.CallPlayerCharacteristicChangeEvent(this);
        }
    }
    private float GetBuffBonus()
    {
        if (extras[2] == 0) return 0.2f;
        if (extras[2] == 1) return 0.5f;
        if (extras[2] == 2) return 0.1f;
        return 0f;
    }
    void OnPlayerKillEnemyEvent(int playerIndex,int _)
    {
        var p = PlayerManager.Instance.GetPlayerByPlayerIndex(playerIndex);
        if (extras[2] == 0) p.ChangeAttack(0.5f);
        if (extras[2] == 1) p.ChangeAttackToNum(1);
        if (extras[2] == 2) return;
            
    }
}

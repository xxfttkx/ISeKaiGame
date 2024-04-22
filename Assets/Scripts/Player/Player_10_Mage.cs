using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_10_Mage : Player_Single
{
    int count;
    protected override void Awake()
    {
        character.index = 10;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
    }
    public override void Reset()
    {
        base.Reset();
        count = 0;
    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        PoolManager.Instance.CreateLetter(this, e, _pos);
        yield break;
    }
    private void OnPlayerKillEnemyEvent(int playerIndex, int _)
    {
        if (playerIndex == GetPlayerIndex() && extras[2] != 2)
        {
            if (count < _maxCount)
            {
                ++count;
                float b = _bonus;
                ApplyBuff("Player10", -1, b, b, b, b, b, ApplyBuffType.Add);
            }
        }
    }
    float _bonus
    {
        get => extras[2] == 1 ? 0.04f : 0.05f;
    }
    int _maxCount
    {
        get => extras[2] == 1 ? 50 : 20;
    }

    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && extraIndex == 2)
        {
            EventHandler.CallPlayerCharacteristicChangeEvent(this);
            AddBuffBeforeStart();
        }
    }
    public override void AddBuffBeforeStart()
    {
        if (_bonusNum == 0)
            this.RemoveBuff("10");
        else
            this.ApplyBuff("10", -1, Characteristic.Attack, _bonusNum, ApplyBuffType.Override);
    }
    int _bonusNum
    {
        get => extras[2] switch
        {
            0 => -5,
            1 => -5,
            2 => 0,
            _ => 0,
        };
    }
}

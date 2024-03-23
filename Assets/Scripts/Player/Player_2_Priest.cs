using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2_Priest : Priest
{
    public GameObject circle;
    protected override void Awake()
    {
        character.index = 2;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
    }
    protected override void OnDisable()
    {
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
        base.OnDisable();
    }
    public override void Reset()
    {
        base.Reset();
    }
    public override void HealSkill()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            if (!p.IsAlive()) continue;
            PlayerManager.Instance.PlayerHealPlayer(character.index, p.character.index);
        }
    }

    public override void AddBuffBeforeStart()
    {
        float b = GetBuffBonus();
        foreach (var p in PlayerManager.Instance.players)
        {
            p.ApplyBuff("player2", -1, b, b, b, b, b, ApplyBuffType.Override);
        }
    }
    private float GetBuffBonus()
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if (extra == 0) return 0.1f;
        if (extra == 1) return 0.5f;
        if (extra == 2) return 0.05f;
        return 0f;
    }
    private void OnPlayerKillEnemyEvent(int playerIndex)
    {
        if(playerIndex==2)
        {
            int extra = extras[1];
            if (extra == 0) return;
            if (extra == 1)
            {
                addHp += 1;
            }
            if (extra == 2) PlayerManager.Instance.PlayerHealPlayer(2,2,1);
        }
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if(playerIndex==GetPlayerIndex() &&extraIndex == 2)
        {
            AddBuffBeforeStart();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_9_Priest : Priest
{
    protected override void Awake()
    {
        character.index = 9;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.ChangePlayerOnTheFieldEvent += ChangePlayerOnTheFieldEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.ChangePlayerOnTheFieldEvent -= ChangePlayerOnTheFieldEvent;
    }

    public override void HealSkill()
    {
        var p = GetHealTarget();
        if(p!=null)
            PlayerManager.Instance.PlayerHealPlayer(GetPlayerIndex(), p.GetPlayerIndex());
    }

    Player GetHealTarget()
    {
        return extras[2] switch
        {
            0=> PlayerManager.Instance.GetPlayerInControl(),
            1=> PlayerManager.Instance.GetMinHpValPlayerUnder(),
            2 => PlayerManager.Instance.GetMinHpValPlayer(),
            _ => null,
        };
    }
    public override void AddBuffBeforeStart()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            p.RemoveBuff("player9");
        }
        if (extras[1]==1)
        {
            var p = PlayerManager.Instance.GetPlayerInControl();
            p.ApplyBuff("player9", -1, 0f, 0f, 0f, GetBuffBonus(), 0f, ApplyBuffType.Override);
        }
        else if (extras[1] == 2)
        {
            foreach (var p in PlayerManager.Instance.players)
            {
                p.ApplyBuff("player9", -1, 0f, 0f, 0f, GetBuffBonus(), 0f, ApplyBuffType.Override);
            }
        }
    }
    private float GetBuffBonus()
    {
        return extras[1] switch
        {
            0 => 0f,
            1 => 0.6f,
            2 => 0.3f,
            _ => 0f,
        };
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && extraIndex == 1)
        {
            AddBuffBeforeStart();
        }
    }
    void ChangePlayerOnTheFieldEvent(Player player)
    {
        if (extras[1] == 1)
        {
            foreach (var p in PlayerManager.Instance.players)
            {
                p.RemoveBuff("player9");
            }
            player.ApplyBuff("player9", -1, 0f, 0f, 0f, GetBuffBonus(), 0f, ApplyBuffType.Override);
        }
    }

}

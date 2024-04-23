using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 吟唱时间长 范围 = 吟唱时间 伤害*=吟唱时间
public class Player_19_Priest : Priest
{
    protected override void Awake()
    {
        character.index = 19;
        base.Awake();
    }
    public override void HealSkill()
    {
        
        var h = GetHp();
        if (h <= 1) return;
        var a = Mathf.CeilToInt(_atk * HurtSelfBonus);
        var num = Mathf.Min(a, h - 1);
        this.BeCompanionHurt(num, 19);
        foreach (var p in PlayerManager.Instance.players)
        {
            if (!p.IsAlive() || p.GetPlayerIndex() == 19) continue;
            PlayerManager.Instance.PlayerHealPlayer(19, p.GetPlayerIndex(), num);
        }
    }
    float HurtSelfBonus
    {
        get => extras[2] switch
        {
            0 => 1f,
            1 => 0.5f,
            2 => 2f,
            _ => 1f,
        };
    }
    float AtkBonus
    {
        get => 0.5f;
    }
    float AtkRangeBonus
    {
        get => 0.5f;
    }
    public override void AddBuffBeforeStart()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            p.RemoveBuff("19_1");
        }
        if (extras[1] == 1)
        {
            foreach (var p in PlayerManager.Instance.players)
            {
                p.ApplyBuff("19_1", -1, AtkBonus, 0f, 0f, 0f, 0f, ApplyBuffType.Override);
            }
            this.ApplyBuff("19_1", -1, -AtkBonus, 0f, 0f, 0f, 0f, ApplyBuffType.Override);
        }
        else if (extras[1] == 2)
        {
            foreach (var p in PlayerManager.Instance.players)
            {
                p.ApplyBuff("19_1", -1, 0f, 0f, 0f, AtkRangeBonus, 0f, ApplyBuffType.Override);
            }
            this.ApplyBuff("19_1", -1, 0f, 0f, 0f, -AtkRangeBonus, 0f, ApplyBuffType.Override);
        }
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && (extraIndex == 1))
        {
            AddBuffBeforeStart();
        }
    }
}

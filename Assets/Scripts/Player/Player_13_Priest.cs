using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_13_Priest : Priest
{
    protected override void Awake()
    {
        character.index = 13;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void HealSkill()
    {
        if(extras[2]==0|| extras[2] == 2)
        {
            var p = PlayerManager.Instance.GetMinHpValPlayer();
            PlayerManager.Instance.PlayerHealPlayer(GetPlayerIndex(), p.GetPlayerIndex(), GetHealNum(p));
        }
        else if (extras[2] == 1)
        {
            foreach(var p in PlayerManager.Instance.players)
            {
                if (p.IsAlive())
                {
                    PlayerManager.Instance.PlayerHealPlayer(GetPlayerIndex(), p.GetPlayerIndex(), GetHealNum(p));
                }
            }
        }


    }

    int GetHealNum(Player p)
    {
        return Mathf.CeilToInt(p.GetHp() * GetHealVal());
    }
    float GetHealVal()
    {
        return extras[2] switch
        {
            0 => 0.3f,
            1 => 0.2f,
            2 => 0.5f,
            _ => 0.3f,
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
    public override void AddBuffBeforeStart()
    {
        if(extras[1]!=0)
        {
            float b = extras[1] == 1 ? 1.0f : -0.8f;
            foreach (var p in PlayerManager.Instance.players)
            {
                p.ApplyBuff("player13_extra1", -1, Characteristic.ProjectileSpeedBonus, b, ApplyBuffType.Override);
            }
        }
        else
        {
            foreach (var p in PlayerManager.Instance.players)
            {
                p.RemoveBuff("player13_extra1");
            }
        }
        
    }
}

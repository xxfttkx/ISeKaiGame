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
        int healIndex = -1;
        float minVal = 1;
        int needHealHp = 0;
        foreach (var p in PlayerManager.Instance.players)
        {
            if (!p.IsAlive()) continue;
            float val = p.GetHpVal();
            if(val < minVal)
            {
                val = minVal;
                healIndex = p.GetPlayerIndex();
                needHealHp = p.character.hp;
            }

        }
        if(healIndex!=-1)
            PlayerManager.Instance.PlayerHealPlayer(character.index, healIndex, needHealHp);
    }

}

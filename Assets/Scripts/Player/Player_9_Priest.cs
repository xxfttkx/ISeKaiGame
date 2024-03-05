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
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void HealSkill()
    {
        var p = PlayerManager.Instance.GetPlayerInControl();
        PlayerManager.Instance.PlayerHealPlayer(GetPlayerIndex(), p.GetPlayerIndex());
    }

}

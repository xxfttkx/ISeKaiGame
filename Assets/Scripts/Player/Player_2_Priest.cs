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
    }
    protected override void OnDisable()
    {
        base.OnDisable();
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
        foreach(var p in PlayerManager.Instance.players)
        {
            p.AddBuff("player2", 0.1f, 0.1f, 0.1f, 0.1f);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_11_Mage : Priest
{
    // ´ó¸»ºÀ
    protected override void Awake()
    {
        character.index = 11;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.PlayerHurtEnemyEvent += OnPlayerHurtEnemyEvent;
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
        EventHandler.EnemyHurtPlayerEvent += OnEnemyHurtPlayerEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.PlayerHurtEnemyEvent -= OnPlayerHurtEnemyEvent;
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
        EventHandler.EnemyHurtPlayerEvent -= OnEnemyHurtPlayerEvent;
    }
    public override void StartAttack()
    {
        // Do Nothing
    }
    void OnPlayerHurtEnemyEvent(int atkIndex, EnemyBase e, int atk)
    {
        HealSomeone();
    }
    void OnPlayerKillEnemyEvent(int playerIndex, int enemyIndex)
    {
        if (extras[1] == 1)
            HealSomeone();
    }
    void OnEnemyHurtPlayerEvent(EnemyBase e, int hurtIndex, int atk)
    {
        if (extras[1] == 2)
            HealSomeone();
    }
    Player healTarget
    {
        get
        {
            if (extras[2] == 1)
                return PlayerManager.Instance.GetMinHpValPlayer();
            else
                return PlayerManager.Instance.GetPlayerInControl();
        }
    }
    int healNum
    {
        get
        {
            return extras[2] == 2 ? 2 : 1;
        }
    }


    void HealSomeone()
    {
        var p = healTarget;
        if (p != null && p.IsAlive())
        {
            PlayerManager.Instance.PlayerHealPlayer(11, p.GetPlayerIndex(), healNum);
        }
    }
}

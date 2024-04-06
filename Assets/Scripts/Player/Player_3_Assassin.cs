using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_3_Assassin : Player
{
    public Player3_Knife knife;
    protected override void Awake()
    {
        character.index = 3;
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
    protected override void OnEnterLevelEvent(int l)
    {
        base.OnEnterLevelEvent(l);
    }
    public override void Reset()
    {
        base.Reset();
    }
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(Attack());
    }
    private void OnPlayerKillEnemyEvent(int playerIndex, int _)
    {
        if (3 == playerIndex)
        {
            int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
            if (extra == 0) PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), GetPlayerIndex(), Mathf.FloorToInt(hp / 2f));
            if (extra == 1) PlayerManager.Instance.PlayerHurtPlayer(GetPlayerIndex(), GetPlayerIndex(), hp - 1);
            if (extra == 2) return;
        }
    }

    IEnumerator Attack()
    {
        while(true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, _range);
            if (e != null)
            {
                Vector2 dis = e._pos - this._pos;
                float r;
                if (extras[1] == 1)
                    r = _range;
                else
                    r = dis.magnitude;
                yield return knife.AttackDir(dis.normalized, r, GetSkillCD());
            }
            else
            {
                yield return null;
            }
        }
    }
    public override void AddBuffBeforeStart()
    {
        float b = GetBuffBonus();
        ApplyBuff("player3", -1, 0f, b, 0f, 0f, 0f, ApplyBuffType.Override);
    }
    private float GetBuffBonus()
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if (extra == 0) return 0.5f;
        if (extra == 1) return 1.0f;
        if (extra == 2) return 0.1f;
        return 0f;
    }
    protected override void OnDesireChangeEvent(int playerIndex, int extraIndex, int selectedIndex)
    {
        base.OnDesireChangeEvent(playerIndex, extraIndex, selectedIndex);
        if (playerIndex == GetPlayerIndex() && extraIndex == 2)
        {
            AddBuffBeforeStart();
        }
    }
}

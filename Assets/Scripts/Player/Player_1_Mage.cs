using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1_Mage : Player_Single
{
    protected override void Awake()
    {
        character.index = 1;
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

    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        PoolManager.Instance.CreateFeather(e, this.transform.position);
        AudioManager.Instance.PlaySoundEffect(SoundName.Atk);
        yield break;
    }
    public override void AddBuffBeforeStart()
    {
        foreach (var p in PlayerManager.Instance.players)
        {
            p.AddBuff("player2", GetBuffBonus(), 0f, 0f, 0f);
        }
    }
    private float GetBuffBonus()
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if (extra == 0) return 0.2f;
        if (extra == 1) return 0.5f;
        if (extra == 2) return 0.1f;
        return 0f;
    }
}

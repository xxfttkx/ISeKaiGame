using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_10_Mage : Player_Single
{
    int count;
    protected override void Awake()
    {
        character.index = 10;
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
    public override void Reset()
    {
        base.Reset();
        count = 0;
    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        PoolManager.Instance.CreateLetter(this, e, _pos);
        AudioManager.Instance.PlaySoundEffect(SoundName.Atk);
        yield break;
    }
    private void OnPlayerKillEnemyEvent(int playerIndex,int _)
    {
        if(playerIndex==GetPlayerIndex()&&extras[2]!=2)
        {
            if(count<_maxCount)
            {
                ++count;
                float b = _bonus;
                ApplyBuff("Player10", -1, b, b, b, b, b, ApplyBuffType.Add);
            }
        }
    }
    float _bonus
    {
        get=> extras[2] == 1 ? 0.04f : 0.05f;
    }
    int _maxCount
    {
        get => extras[2] == 1 ? 50 : 20;
    }
    public override int GetRawAtk()
    {
        return base.GetRawAtk() + (extras[2] == 2 ? 0 : -5);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_16_Assassin : Player_Single
{
    protected override void Awake()
    {
        character.index = 16;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    public override void Reset()
    {
        base.Reset();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        PoolManager.Instance.CreateLightning(this,e,this.transform.position);
        yield break;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_12_Mage : Player_Single
{

    protected override void Awake()
    {
        character.index = 12;
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
        PoolManager.Instance.ThrowBianPao(e, this);
        yield break;
    }
}

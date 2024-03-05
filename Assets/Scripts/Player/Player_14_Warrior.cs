using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_14_Warrior : Player_Area
{
    protected override void Awake()
    {
        character.index = 14;
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
    protected override IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        yield break;
    }
    protected override void AttackEnemies(List<EnemyBase> _)
    {
        return;
    }
}

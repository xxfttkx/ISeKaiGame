using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_15_Warrior : Player_Area
{
    private int count;
    protected override void Awake()
    {
        character.index = 15;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    public override void Reset()
    {
        base.Reset();
        count = 0;
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
    public override void BeHurt(int attack)
    {
        if(count==5)
        {
            count = 0;
            return;
        }
        base.BeHurt(attack);
        count++;
    }
}

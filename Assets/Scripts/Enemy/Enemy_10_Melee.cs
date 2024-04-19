using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_10_Melee : Enemy_Melee_Idle
{
    protected override void Awake()
    {
        enemy.index = 10;
        base.Awake();
    }
    public override void AttackPlayer()
    {
        base.AttackPlayer();
    }
}

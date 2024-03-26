using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0_Melee : Enemy_Melee
{
    protected override void Awake()
    {
        enemy.index = 0;
        base.Awake();
    }
    public override void AttackPlayer()
    {
        base.AttackPlayer();
    }
}

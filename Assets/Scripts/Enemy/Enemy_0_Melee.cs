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

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void AttackPlayer()
    {
        PlayerManager.Instance.EnemyHurtPlayer(this, player);
    }
}

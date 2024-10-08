using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1_Ranged : Enemy_Ranged
{

    protected override void Awake()
    {
        enemy.index = 1;
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

    public override void AttackPlayer()
    {
        Vector2 dir = player.transform.position - this.transform.position;
        PoolManager.Instance.CreateGuangQiu(dir, this);
    }

}

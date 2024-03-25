using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged_3 : Enemy_Ranged
{
    protected override void Awake()
    {
        enemy.index = 3;
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
        Vector2 dir = player.transform.position - this.transform.position;
        PoolManager.Instance.CreateGuangQiu(dir, this, 1, GetGuangQiuSpeed());
    }
    private float GetGuangQiuSpeed()
    {
        return 30f;
    }
}

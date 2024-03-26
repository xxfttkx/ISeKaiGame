using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2_Ranged : Enemy_Ranged
{
    protected override void Awake()
    {
        enemy.index = 2;
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
        Quaternion rotation1 = Quaternion.Euler(0, 0, 30);
        Quaternion rotation2 = Quaternion.Euler(0, 0, -30);
        Vector2 rotatedDir1 = rotation1 * dir;
        Vector2 rotatedDir2 = rotation2 * dir;
        PoolManager.Instance.CreateGuangQiu(rotatedDir1, this);
        PoolManager.Instance.CreateGuangQiu(rotatedDir2, this);
    }
}

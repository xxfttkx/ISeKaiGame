using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_11_Ranged : Enemy_Ranged
{

    protected override void Awake()
    {
        enemy.index = 11;
        base.Awake();
    }

    public override void AttackPlayer()
    {
        Vector2 dir = player.transform.position - this.transform.position;
        float randomAngle = Random.Range(-100f, 100f);
        dir = Utils.GetVec2RotateAngle(dir, randomAngle);
        PoolManager.Instance.CreateGuangQiu(dir, this);
    }

}

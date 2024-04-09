using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_13_Ranged : Enemy_Ranged_Idle
{
    protected override void Awake()
    {
        enemy.index = 13;
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
        float randomAngle = Random.Range(-100f, 100f);
        dir = Utils.GetVec2RotateAngle(dir, randomAngle);
        PoolManager.Instance.CreateGuangQiu(dir, this, 1, GetGuangQiuSpeed());
    }
    private float GetGuangQiuSpeed()
    {
        return 7f * levelBonus;
    }
}

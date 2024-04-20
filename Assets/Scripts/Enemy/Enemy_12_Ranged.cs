using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_12_Ranged : Enemy_Ranged
{
    protected override void Awake()
    {
        enemy.index = 12;
        base.Awake();
    }
    public override void AttackPlayer()
    {
        Vector2 dir = player.transform.position - this.transform.position;
        float randomAngle = Random.Range(-100f, 100f);
        dir = Utils.GetVec2RotateAngle(dir, randomAngle);
        Quaternion rotation1 = Quaternion.Euler(0, 0, 30);
        Quaternion rotation2 = Quaternion.Euler(0, 0, -30);
        Vector2 rotatedDir1 = rotation1 * dir;
        Vector2 rotatedDir2 = rotation2 * dir;
        PoolManager.Instance.CreateGuangQiu(rotatedDir1, this);
        PoolManager.Instance.CreateGuangQiu(rotatedDir2, this);
    }
}

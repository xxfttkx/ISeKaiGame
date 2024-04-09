using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_16_Ranged : Enemy_Ranged_Idle
{
    protected override void Awake()
    {
        enemy.index = 16;
        base.Awake();
    }

    public override void AttackPlayer()
    {
        Vector2 dir = player.transform.position - this.transform.position;
        float randomAngle = Random.Range(-100f, 100f);
        for (int angle = -20;angle<=20; angle+=10)
        {
            Vector2 d = Utils.GetVec2RotateAngle(dir, angle + randomAngle);
            PoolManager.Instance.CreateGuangQiu(d, this, 0.3f, GetGuangQiuSpeed());
        }
        
    }
    private float GetGuangQiuSpeed()
    {
        return 5f * levelBonus;
    }

}

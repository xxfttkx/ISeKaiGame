using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_6_Ranged : Enemy_Ranged
{
    protected override void Awake()
    {
        enemy.index = 6;
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
        
        for(int angle = -20;angle<=20; angle+=10)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 d = rotation * dir;
            PoolManager.Instance.CreateGuangQiu(d, this, 0.3f, GetGuangQiuSpeed());
        }
        
    }
    private float GetGuangQiuSpeed()
    {
        return 10f;
    }

}

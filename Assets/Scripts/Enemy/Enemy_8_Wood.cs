using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_8_Melee : EnemyBase
{
    protected override void Awake()
    {
        enemy.index = 8;
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
    public override void Reset()
    {
        base.Reset();
    }
}

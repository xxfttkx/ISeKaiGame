using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4_Melee : Enemy_Melee
{
    public float existTime;
    protected override void Awake()
    {
        enemy.index = 4;
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        if(existTime<10.0f)
            existTime += Time.deltaTime;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void Reset()
    {
        base.Reset();
        existTime = 0;
    }
    protected override void AttackPlayer()
    {
        PlayerManager.Instance.EnemyHurtPlayer(this, player);
    }

    public override int GetSpeed()
    {
        float s = base.GetSpeed();
        s = Mathf.Lerp(1, s, existTime / 10.0f);
        return Mathf.CeilToInt(s);
    }
}

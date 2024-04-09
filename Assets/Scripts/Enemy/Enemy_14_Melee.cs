using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_14_Melee : Enemy_Melee_Idle
{
    public float existTime;
    protected override void Awake()
    {
        enemy.index = 14;
        base.Awake();
    }

    public override void Reset()
    {
        base.Reset();
        existTime = 0;
        StartCoroutine(CountTime());
    }
    IEnumerator CountTime()
    {
        while(true)
        {
            if (existTime >= 10.0f) yield break;
            existTime += Time.deltaTime;
            yield return null;
        }
        
    }
    public override void AttackPlayer()
    {
        PlayerManager.Instance.EnemyHurtPlayer(this, player);
    }

    public override float GetSpeed()
    {
        float s = base.GetSpeed();
        s = Mathf.Lerp(1, s, existTime / 10.0f);
        return s;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Single : Player
{
    protected override void OnEnterLevelEvent(int l)
    {
        base.OnEnterLevelEvent(l);
        StartCoroutine(Attack());
    }
    protected virtual IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, GetAttackRange());
            if (e == null)
            {
                yield return null;
                continue;
            }
            yield return AttackAnim(e);
            var cd = new WaitForSeconds(10.0f / GetAttackSpeed());
            yield return cd;
        }

    }
    protected virtual IEnumerator AttackAnim(EnemyBase e)
    {
        yield break;
    }
}

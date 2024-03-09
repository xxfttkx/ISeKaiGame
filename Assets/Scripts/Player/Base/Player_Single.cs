using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Single : Player
{
    public override void Reset()
    {
        base.Reset();
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
            yield return new WaitForSeconds(GetSkillCD());
        }

    }
    protected virtual IEnumerator AttackAnim(EnemyBase e)
    {
        yield break;
    }
}

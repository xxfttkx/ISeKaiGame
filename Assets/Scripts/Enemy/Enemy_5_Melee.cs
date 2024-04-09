using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_5_Melee : Enemy_Melee
{
    protected override void Awake()
    {
        enemy.index = 5;
        base.Awake();
    }
    protected override IEnumerator Attack()
    {
        yield return null;
        while (true)
        {
            if (CanAttack && (player._pos - _pos).sqrMagnitude <= GetSqrAttackRange())
            {
                yield return AttackAnim();
                yield return new WaitForSeconds(GetSkillCD());
            }
            else
                yield return null;
        }
    }

    IEnumerator AttackAnim()
    {
        inAtkAnim = true;
        Vector2 dir = player.transform.position - this.transform.position;
        dir = dir.normalized;
        IsMoving = false;
        yield return new WaitForSeconds(WaitChongCiTime());
        if (!IsAlive()) yield break;
        bool bAttackSuccess = false;
        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            if (player == null)
            {
                rb.velocity = Vector2.zero;
                break;
            }
            IsMoving = true;
            rb.velocity = dir * GetSpeed() * 5;
            if (!bAttackSuccess)
            {
                Vector2 diff = player._pos - _pos;
                if (diff.sqrMagnitude < Settings.hitPlayerDis * Settings.hitPlayerDis)
                {
                    bAttackSuccess = true;
                    PlayerManager.Instance.EnemyHurtPlayer(this);
                }
            }
            yield return null;
        }
        inAtkAnim = false;
    }
    float WaitChongCiTime()
    {
        return 1.0f / levelBonus;
    }
}

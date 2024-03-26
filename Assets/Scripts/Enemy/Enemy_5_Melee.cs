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
        bool bAttackSuccess = false;
        for (float t = 0; t < 0.5; t += Time.deltaTime)
        {
            IsMoving = true;
            rb.MovePosition(rb.position + dir * GetSpeed() * 5 * Time.deltaTime);
            if (!bAttackSuccess)
            {
                float dis = Utils.GetSqrDisWithPlayer(this.transform);
                if (dis < Settings.hitPlayerDis * Settings.hitPlayerDis)
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

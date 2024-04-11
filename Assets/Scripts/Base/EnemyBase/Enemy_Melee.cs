using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : EnemyBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override IEnumerator Attack()
    {
        yield return null;
        while (true)
        {
            if (CanAttack&&(player._pos - _pos).sqrMagnitude <= GetSqrAttackRange())
            {
                AttackPlayer();
                yield return new WaitForSeconds(GetSkillCD());
            }
            else
                yield return null;
        }
    }

    protected override IEnumerator MoveToPlayer()
    {
        while (true)
        {
            if(CanMove)
            {
                Vector2 deltaPos = player._pos - _pos;
                float distance = deltaPos.magnitude;
                if (movementVec2.x < 0) sp.flipX = !enemy.creature.faceToLeft;
                else if (movementVec2.x > 0) sp.flipX = enemy.creature.faceToLeft;
                if (distance < 0.1f)
                {
                    IsMoving = false;
                }
                else
                {
                    IsMoving = true;
                    rb.velocity = movementVec2 * GetSpeed();
                }
            }
            else
            {
                IsMoving = false;
            }
            yield return null;
        }
    }
}

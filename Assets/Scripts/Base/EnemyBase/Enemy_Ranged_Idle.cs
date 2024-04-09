using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Ranged_Idle : EnemyBase
{
    protected override IEnumerator Attack()
    {
        yield return null;
        while (true)
        {
            EnemyManager.Instance.AddEnemyToAtkQue(this);
            yield return new WaitForSeconds(GetSkillCD());
        }
    }

    protected override IEnumerator MoveToPlayer()
    {

        while (true)
        {
            if (CanMove)
            {
                Vector2 deltaPos = player._pos - _pos;
                float distance = deltaPos.magnitude;
                if (movementVec2.x < 0) sp.flipX = !enemy.creature.faceToLeft;
                else if (movementVec2.x > 0) sp.flipX = enemy.creature.faceToLeft;
                if (distance < GetAttackRange() - 2)
                {
                    IsMoving = true;
                    rb.velocity = -movementVec2 * GetSpeed();
                }
                else if (distance > GetAttackRange() - 1)
                {
                    IsMoving = true;
                    rb.velocity = movementVec2 * GetSpeed();
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    IsMoving = false;
                }
            }
            else
            {
                IsMoving = false;
            }
            yield return new WaitForSeconds(enemy.getPlayerPosTimeDelta);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee_Idle : EnemyBase
{
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
                    rb.velocity = Vector2.zero;
                    IsMoving = false;
                }
                else
                {
                    IsMoving = true;
                    float x = movementVec2.x;
                    float y = movementVec2.y;
                    if(Mathf.Abs(x)>Mathf.Abs(y))
                    {
                        rb.velocity = new Vector2(x * GetSpeed(),0);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, y * GetSpeed());
                    }
                   
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

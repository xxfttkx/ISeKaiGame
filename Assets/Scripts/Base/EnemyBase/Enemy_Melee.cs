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
                if (movementVec2.x < 0) sp.flipX = !enemy.faceToLeft;
                else if (movementVec2.x > 0) sp.flipX = enemy.faceToLeft;
                if (distance < 0.1f)
                {
                    rb.velocity = Vector2.zero;
                    IsMoving = false;
                }
                else
                {
                    IsMoving = true;
                    rb.MovePosition(rb.position + movementVec2 * GetSpeed() * Time.deltaTime);
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

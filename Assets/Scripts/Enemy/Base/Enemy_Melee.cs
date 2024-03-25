using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : EnemyBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(MeleeAttack());
    }
    protected virtual IEnumerator MeleeAttack()
    {
        yield return null;
        while (true)
        {
            if (!IsAlive()) break;
            bool attackSuccess = false;
            do
            {
                if (isBegingRepelled) break;
                if (player == null) break;
                Vector2 dis = player.transform.position - this.transform.position;
                if (dis.sqrMagnitude > GetSqrAttackRange()) break;
                attackSuccess = true;
            } while (false);
            if (!attackSuccess)
            {
                yield return null;
                continue;
            }
            else
            {
                AttackPlayer();
                yield return new WaitForSeconds(10 / GetAttackSpeed());
            }

        }
    }

    protected override IEnumerator MoveToPlayer()
    {
        while (true)
        {
            if(!canMove|| isBegingRepelled)
            {
                yield return null;
                continue;
            }
            if (!IsAlive() || player == null) yield break;
            Vector2 deltaPos = player.transform.position - this.transform.position;
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

            yield return null;
        }
    }
}

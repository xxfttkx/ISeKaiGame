using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Ranged : EnemyBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(YuanChengAttack());
    }

    
    IEnumerator YuanChengAttack()
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
                float randomOffset = GetRandomOffset();
                yield return new WaitForSeconds(10 / GetAttackSpeed() + randomOffset);
            }

        }
    }

    protected override IEnumerator MoveToPlayer()
    {
        while (true)
        {
            if (isBegingRepelled) yield return new WaitForSeconds(0.1f);
            if (!IsAlive()) yield break;
            if (player == null)
            {
                yield return null;
                continue;
            }
            Vector2 deltaPos = player.transform.position - this.transform.position;
            float distance = deltaPos.magnitude;
            if (movementVec2.x < 0) sp.flipX = !enemy.faceToLeft;
            else if (movementVec2.x > 0) sp.flipX = enemy.faceToLeft;
            if (distance < GetAttackRange() - 2)
                rb.MovePosition(rb.position - movementVec2 * enemy.speed * moveDelta);
            else if (distance > GetAttackRange() - 1)
                rb.MovePosition(rb.position + movementVec2 * enemy.speed * moveDelta);
            else
                rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(moveDelta);
        }
    }

}

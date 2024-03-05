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

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(MeleeAttack());
    }
    protected override IEnumerator MeleeAttack()
    {
        yield return null;
        while (true)
        {
            if (!IsAlive()) break;
            bool attackSuccess = false;
            do
            {
                if (isBegingRepelled) break;
                player = PlayerManager.Instance.GetPlayerInControl();
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
                yield return Attack();
                yield return new WaitForSeconds(10 / GetAttackSpeed());
            }

        }
    }

    IEnumerator Attack()
    {
        canMove = false;
        Vector2 dir = player.transform.position - this.transform.position;
        dir = dir.normalized;
        yield return new WaitForSeconds(1.0f);
        bool bAttackSuccess = false;
        for (float t = 0; t < 0.5; t += moveDelta)
        {
            rb.MovePosition(rb.position + dir * GetSpeed()*5 * moveDelta);
            if(!bAttackSuccess)
            {
                float dis = Utils.GetSqrDisWithPlayer(this.transform);
                if(dis<0.6*0.6)
                {
                    bAttackSuccess = true;
                    PlayerManager.Instance.EnemyHurtPlayer(this);
                }
            }
            yield return new WaitForSeconds(moveDelta);
        }
        canMove = true;
    }

}

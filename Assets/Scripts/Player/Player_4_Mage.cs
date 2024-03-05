using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_4_Mage : Player
{

    protected override void Awake()
    {
        character.index = 4;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Attack());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position,GetAttackRange());
            if(e==null)
            {
                yield return null;
                continue;
            }
            PoolManager.Instance.CreateBubble(e, this.transform.position, this);
            var cd = new WaitForSeconds(10.0f / GetAttackSpeed());
            yield return cd;
        }
    }
}

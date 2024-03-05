using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_6_Mage : Player
{

    protected override void Awake()
    {
        character.index = 6;
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
            var e = Utils.GetNearestEnemy(this.transform.position, this.GetAttackRange());
            if (e != null)
            {
                PoolManager.Instance.CreateChicken(this.transform.position);
                var cd = new WaitForSeconds(10.0f / GetAttackSpeed());
                yield return cd;
            }
            else
            {
                yield return null;
            }
        }
    }
}

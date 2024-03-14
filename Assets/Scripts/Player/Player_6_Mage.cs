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
            if(extras[2]==2)
            {
                Vector2 pos = this.transform.position;
                pos.x -= 2;
                PoolManager.Instance.CreateChicken(pos);
                pos.x += 4;
                PoolManager.Instance.CreateChicken(pos);
            }
            else
            {
                PoolManager.Instance.CreateChicken(this.transform.position);
            }
            
            yield return new WaitForSeconds(GetSkillCD());
        }
    }
}

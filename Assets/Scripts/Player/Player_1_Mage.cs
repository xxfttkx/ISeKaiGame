using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1_Mage : Player
{

    protected override void Awake()
    {
        character.index = 1;
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
            if(e!=null)
            {
                PoolManager.Instance.CreateFeather(e, this.transform.position);
                AudioManager.Instance.PlaySoundEffect(SoundName.Atk);
                var cd = 10.0f / GetAttackSpeed();
                yield return new WaitForSeconds(cd);
            }
            else
            {
                yield return null;
            }
            
        }

    }
}

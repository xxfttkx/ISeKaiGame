using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_11_Mage : Player_Single
{
    // ´ó¸»ºÀ
    protected override void Awake()
    {
        character.index = 11;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Daifugo());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
        AudioManager.Instance.PlaySoundEffect(SoundName.Atk);
        yield break;
    }
    protected IEnumerator Daifugo()
    {
        while(true)
        {
            yield return new WaitForSeconds(10.0f);
            List<int> list = new List<int> { atk, speed, atkSpeed, atkRange };
            Utils.ShuffleArray(list);
            atk = list[0];
            speed = list[1];
            atkSpeed = list[2];
            atkRange = list[3];
        }
    }
}

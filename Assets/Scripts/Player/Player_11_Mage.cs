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
            List<int> list = new List<int> { character.attack, character.speed, character.attackSpeed, character.attackRange };
            Utils.ShuffleArray(list);
            character.attack = list[0];
            character.speed = list[1];
            character.attackSpeed = list[2];
            character.attackRange = list[3];
        }
    }
}

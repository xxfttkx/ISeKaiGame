using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_10_Mage : Player_Single
{

    protected override void Awake()
    {
        character.index = 10;
        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
    }
    protected override IEnumerator AttackAnim(EnemyBase e)
    {
        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
        StartCoroutine(StealHeart(e));
        AudioManager.Instance.PlaySoundEffect(SoundName.Atk);
        yield break;
    }
    IEnumerator StealHeart(EnemyBase e)
    {
        int globalIndex = e.GetGlobalIndex();
        while(true)
        {
            int index = e.GetGlobalIndex();
            if(index!=globalIndex)
            {
                Debug.Log("À¿¡À°£°£°£°£");
                break;
            }
            int h = e.GetHP();
            PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e , GetStealHeartAtk());
            yield return new WaitForSeconds(GetStealDelta());
        }
    }
    private void OnPlayerKillEnemyEvent(int playerIndex)
    {
        if(playerIndex==GetPlayerIndex())
        {
            AddBuff("Player10", 0.1f);
        }
    }
    private float GetStealDelta()
    {
        if (extras[1] == 2) return 0.5f;
        return 1.0f;
    }
    private int GetStealHeartAtk()
    {
        return 1;
    }
}

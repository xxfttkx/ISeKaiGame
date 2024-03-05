using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_3_Assassin : Player
{
    public GameObject knife;
    protected override void Awake()
    {
        character.index = 3 ;
        base.Awake();
        knife.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Attack());
        EventHandler.PlayerKillEnemyEvent += OnPlayerKillEnemyEvent;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHandler.PlayerKillEnemyEvent -= OnPlayerKillEnemyEvent;
        
    }
    private void OnPlayerKillEnemyEvent(int playerIndex)
    {
        BeHurt(Mathf.FloorToInt(character.hp / 2f));
    }
    IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, GetAttackRange());
            if (e == null)
            {
                yield return null;
                continue;
            }
            yield return AttackAnim(e);
            var cd = new WaitForSeconds(10.0f / GetAttackSpeed());
            yield return cd;
        }

    }
    IEnumerator AttackAnim(EnemyBase enemy)
    {
        Vector2 dir = enemy.transform.position - this.transform.position;
        float angle = Vector2.Angle(Vector2.up, dir);
        if (dir.x > 0)
        {
            angle = -angle;
        }
        knife.transform.rotation = Quaternion.Euler(0, 0, angle);
        knife.SetActive(true);
        for(float i = 0;i<1;i+=0.1f)
        {
            knife.transform.localPosition = Vector2.Lerp(Vector2.zero, dir, i);

            yield return new WaitForSeconds(0.01f);
        }
        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), enemy);
        for (float i = 0; i < 1; i += 0.1f)
        {
            knife.transform.localPosition = Vector2.Lerp(dir, Vector2.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        // yield return new WaitForSeconds(0.2f);
        knife.SetActive(false);
    }
}

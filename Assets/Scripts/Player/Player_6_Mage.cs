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

    public override void StartAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (extras[2] == 2)
            {
                Vector2 pos = _pos;
                
                CreateChicken(pos);
                CreateChicken(pos);
            }
            else
            {
                CreateChicken(this.transform.position);
            }

            yield return new WaitForSeconds(GetSkillCD());
        }
    }

    void CreateChicken(Vector2 pos)
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        pos += new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        int cost = 0;
        if (extras[3] == 0)cost = 10;
        else if (extras[3] == 1)cost = 20;
        else if (extras[3] == 2)cost = 0;
        Player p;
        if (extras[2] == 1) cost /= 2;
        p = PlayerManager.Instance.GetMaxHpPlayer();
        PlayerManager.Instance.PlayerHurtPlayer(7, p.GetPlayerIndex(), cost);
        PoolManager.Instance.CreateChicken(pos);
    }
}

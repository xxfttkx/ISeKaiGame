using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_8_Warrior : Player_Area
{
    public SpriteRenderer water;
    protected override void Awake()
    {
        character.index = 8;
        base.Awake();
        
    }
    public override void Reset()
    {
        base.Reset();
        water.enabled = false;
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 0);
        if (extra == 1)
        {
            character.attack += 1;
            character.attackRange -= 1;
        }
        else if (extra == 2)
        {
            character.attack -= 1;
            character.attackRange += 1;
        }
    }
    protected override IEnumerator AttackAnim(List<EnemyBase> enemies)
    {
        Player p=null;
        int extra1 = SaveLoadManager.Instance.GetPlayerExtra(8, 2);
        if (extra1==1)
        {
            p = PlayerManager.Instance.GetPlayerInControl();
        }
        else if(extra1==2)
        {
            p = PlayerManager.Instance.GetMinHpValPlayer();
        }
        int a = PlayerManager.Instance.GetPlayerAttack(8);
        int remain = 0;
        int extra2 = SaveLoadManager.Instance.GetPlayerExtra(8, 2);
        if(extra1!=0&&extra2 == 2)
        {
            remain = a * enemies.Count;
            PlayerManager.Instance.PlayerHealPlayer(8,p.GetPlayerIndex(),remain);
            yield break;
        }
        int r = PlayerManager.Instance.GetPlayerAttackRange(8);
        r *= 2;
        water.size = new Vector2(r, r);
        water.enabled = true;
        foreach(var e in enemies)
        {
            int max = e.GetHP();
            if (extra2 != 1) max -= 1;
            if (max == 0) remain += a;
            else if(a<=max) PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e, a);
            else
            {
                PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e, max);
                remain += (a - max);
            }
        }
        if(extra1!=0)
        {
            PlayerManager.Instance.PlayerHealPlayer(8, p.GetPlayerIndex(), remain);
        }
        StartCoroutine(Delay(0.5f));
        yield break;
    }
    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        water.enabled = false;
    }
    public override void BeHurt(int attack)
    {
        int def = 1;
        int extra2 = SaveLoadManager.Instance.GetPlayerExtra(8, 2);
        if (extra2 == 1) def = 0;
        else if (extra2 == 2) def = 3;
        base.BeHurt(attack - def);
    }
}

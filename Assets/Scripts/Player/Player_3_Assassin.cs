using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_3_Assassin : Player
{
    public List<GameObject> knives;
    private HashSet<int> atkingEnemies;
    protected override void Awake()
    {
        character.index = 3 ;
        base.Awake();
        atkingEnemies = new HashSet<int>();
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
    protected override void OnEnterLevelEvent(int l)
    {
        base.OnEnterLevelEvent(l);
    }
    public override void Reset()
    {
        base.Reset();
        atkingEnemies.Clear();

        foreach (var go in knives)
        {
            go.SetActive(false);
        }
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 1);
        if(extra==0)StartCoroutine(Attack());
        else if (extra == 1)
        {
            StartCoroutine(AttackExtra1(0));
            StartCoroutine(AttackExtra1(1));
        }
        else if (extra == 2)
        {
            StartCoroutine(AttackExtra2(0));
            StartCoroutine(AttackExtra2(1));
        }
    }
    private void OnPlayerKillEnemyEvent(int playerIndex)
    {
        if (3 == playerIndex)
        {
            int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
            if (extra == 0) BeHurt(Mathf.FloorToInt(hp / 2f));
            if (extra == 1) BeHurt(hp - 1);
            if (extra == 2) return;
        }
    }
        
    IEnumerator Attack()
    {
        while (true)
        {
            var e = Utils.GetNearestEnemy(this.transform.position, GetAttackRange());
            if (e != null)
            {
                yield return AttackAnim(e, knives[0]);
                yield return new WaitForSeconds(GetSkillCD());
                continue;
            }
            yield return null;
            continue;
        }
    }
    IEnumerator AttackExtra1(int index)
    {
        while (true)
        {
            var enemies = Utils.GetNearEnemiesByDistance(this.transform.position, GetAttackRange());
            if (enemies != null && enemies.Count > 0)
            {
                foreach (var e in enemies)
                {
                    if((e.transform.position.y-this.transform.position.y)*(0.5f-index)>=0)
                    {
                        yield return AttackAnim(e, knives[index]);
                        break;
                    }
                }
                yield return new WaitForSeconds(GetSkillCD());
                continue;
            }
            yield return null;
            continue;
        }
    }
    IEnumerator AttackExtra2(int index)
    {
        while (true)
        {
            var enemies = Utils.GetNearEnemiesByDistance(this.transform.position, GetAttackRange());
            if (enemies != null && enemies.Count > 0)
            {
                int a = PlayerManager.Instance.GetPlayerAttack(3);
                foreach (var e in enemies)
                {
                    if (atkingEnemies.Contains(e.GetGlobalIndex())) continue;
                    if (a < e.GetHP())
                    {
                        atkingEnemies.Contains(e.GetGlobalIndex());
                        yield return AttackAnim(e, knives[index]);
                        atkingEnemies.Remove(e.GetGlobalIndex());
                        break;
                    }
                }
                yield return new WaitForSeconds(GetSkillCD());
                continue;
            }
            yield return null;
            continue;
        }
    }
    IEnumerator AttackAnim(EnemyBase e, GameObject knife)
    {
        
        Vector2 dir = e.transform.position - this.transform.position;
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
        PlayerManager.Instance.PlayerHurtEnemy(GetPlayerIndex(), e);
        for (float i = 0; i < 1; i += 0.1f)
        {
            knife.transform.localPosition = Vector2.Lerp(dir, Vector2.zero, i);
            yield return new WaitForSeconds(0.01f);
        }
        // yield return new WaitForSeconds(0.2f);
        knife.SetActive(false);
        
    }
    public override void AddBuffBeforeStart()
    {
        float b = GetBuffBonus();
        this.AddBuff("player3", 0, b, 0, 0);
    }
    private float GetBuffBonus()
    {
        int extra = SaveLoadManager.Instance.GetPlayerExtra(GetPlayerIndex(), 2);
        if (extra == 0) return 0.5f;
        if (extra == 1) return 1.0f;
        if (extra == 2) return 0.1f;
        return 0f;
    }
}

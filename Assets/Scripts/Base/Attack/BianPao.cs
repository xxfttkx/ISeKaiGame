using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BianPao : PlayerAtk
{
    public float _explosionRange
    {
        get => extras[3] switch
        {
            0 => 1f,
            1 => 2f,
            2 => 0.5f,
            _ => 1f,
        };
    }
    protected override void Awake()
    {
        base.Awake();
        //velocity = 10;
        poolIndex = 4;
        autoReleaseTime = -1;
    }

    protected override IEnumerator AttackEnemy(EnemyBase e)
    {
        Vector2 start = this.transform.position;
        Vector2 target = e.transform.position;
        Vector2 dir = target - start;
        bool hidarimuki = dir.x > 0;
        for (int i = 0; i < 10; i += 1)
        {
            _pos = Vector2.Lerp(start, target, i * 0.1f);
            this.transform.rotation = Quaternion.Euler(0, 0, (i) * 36 * (hidarimuki ? 1 : -1));
            yield return new WaitForSeconds(0.01f);
        }
        Burn();
    }
    public void Burn()
    {
        StartCoroutine(StartExplosion());
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(GetExplosionTime());
        PoolManager.Instance.BianPaoExplosion(_pos, _explosionRange);
        var enemies = Utils.GetNearEnemies(_pos, _explosionRange);
        if(enemies!=null)
        {
            foreach (var e in enemies)
            {
                PlayerManager.Instance.PlayerHurtEnemy(12, e, GetAtk());
            }
        }
        var p = PlayerManager.Instance.GetPlayerInControl();
        if(Utils.CanAttackPlayer(this.gameObject, p, _explosionRange))
            PlayerManager.Instance.PlayerHurtPlayer(12, p.character.index, GetAtkPlayerVal());
        Release();
    }
    float GetExplosionTime()
    {
        return extras[1] switch
        { 
            0=>0.4f,
            1=>0.1f,
            2=>1.0f,
            _=>0.4f,
        };
    }
    float GetAtkTimeBonus()
    {
        return extras[1] switch
        {
            0 => 1f,
            1 => 1f,
            2 => 1.5f,
            _ => 1f,
        };
    }
    float GetAtkRangeBonus()
    {
        return extras[3] switch
        {
            0 => 1f,
            1 => 1f,
            2 => 1.5f,
            _ => 1f,
        };
    }
    int GetAtkPlayerVal()
    {
        return extras[2] switch
        {
            0 => GetAtk(),
            1 => int.MaxValue,
            2 => 0,
            _ => GetAtk(),
        };
    }
    int GetAtk()
    {
        return Mathf.CeilToInt(atk * GetAtkTimeBonus()* GetAtkRangeBonus());
    }
}

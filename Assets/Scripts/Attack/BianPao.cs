using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BianPao : PlayerAtk
{
    private Animator animator;
    public float explosionRange = 1f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override IEnumerator AttackEnemy(EnemyBase e)
    {
        atk = Mathf.CeilToInt(atk*GetAtkBonus());
        Vector2 start = this.transform.position;
        Vector2 target = e.transform.position;
        Vector2 dir = target - start;
        bool hidarimuki = dir.x > 0;
        for (int i = 0; i < 10; i += 1)
        {
            this.transform.position = Vector2.Lerp(start, target, i * 0.1f);
            this.transform.rotation = Quaternion.Euler(0, 0, (i) * 36 * (hidarimuki ? 1 : -1));
            yield return new WaitForSeconds(0.01f);
        }
        Burn();
    }
    public void Burn()
    {
        animator.SetBool("start", true);
        StartCoroutine(StartExplosion());
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(GetExplosionTime());
        animator.SetBool("start", false);
        PoolManager.Instance.BianPaoExplosion(this.transform.position);
        var enemyIndexMask = LayerMask.GetMask("Enemy");
        var playerIndexMask = LayerMask.GetMask("Player");
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(this.transform.position, explosionRange, enemyIndexMask);
        foreach (var coll in enemyColliders)
        {
            if (coll.gameObject.CompareTag("Enemy"))
            {
                var e = coll.gameObject.GetComponent<EnemyBase>();
                PlayerManager.Instance.PlayerHurtEnemy(12,e,atk);
            }
        }
        var p = PlayerManager.Instance.GetPlayerInControl();
        if(Utils.CanAttackPlayer(this.gameObject, p, explosionRange))
            PlayerManager.Instance.PlayerHurtPlayer(12, p.character.index, GetAtkPlayerVal());

        PoolManager.Instance.ReleaseObj(this.gameObject,4);
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
    float GetAtkBonus()
    {
        return extras[1] switch
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
            0 => atk,
            1 => int.MaxValue,
            2 => 0,
            _ => atk,
        };
    }
}

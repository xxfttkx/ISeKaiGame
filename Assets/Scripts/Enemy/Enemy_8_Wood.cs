using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_8_Melee : EnemyBase
{
    public List<GameObject> tentacles;
    protected override void Awake()
    {
        enemy.index = 8;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var t in tentacles)
        {
            StartCoroutine(Attack(t));
        }
    }
    protected override void Reset()
    {
        base.Reset();
        foreach (var t in tentacles)
        {
            var sp = t.GetComponent<SpriteRenderer>();
            sp.size = new Vector2(base.sp.size.x, 0);
        }
    }
    IEnumerator Attack(GameObject tentacle)
    {
        while (true)
        {
            if (isBegingRepelled) yield return new WaitForSeconds(0.1f);
            if (!GameStateManager.Instance.InGamePlay()) yield return new WaitForSeconds(0.1f);
            float nearestDistance = float.MaxValue;
            float currDistance;
            Player nearestPlayer = null;
            foreach (var p in PlayerManager.Instance.players)
            {
                if (!p.IsAlive()) continue;
                currDistance = Vector2.Distance(p.transform.position, tentacle.transform.position);
                if (currDistance < nearestDistance)
                {
                    nearestDistance = currDistance;
                    nearestPlayer = p;
                }
            }
            if (nearestPlayer != null && nearestDistance < enemy.attackRange)
            {
                //ATK
                var dir = nearestPlayer.transform.position - tentacle.transform.position;
                var angle = Vector2.Angle(Vector2.up, dir);
                if (dir.x > 0) angle = -angle;
                tentacle.transform.rotation = Quaternion.Euler(0, 0, angle);
                yield return Stretch(tentacle, nearestDistance, nearestPlayer);
            }
            else
            {

            }
            yield return new WaitForSeconds(10.0f / enemy.attackSpeed);
        }
    }

    IEnumerator Stretch(GameObject tentacle, float nearestDistance, Player player)
    {
        // 0.1f 1->2
        var sp = tentacle.GetComponent<SpriteRenderer>();
        Vector2 diff;
        for (float height = 0; height < nearestDistance; height += 0.2f)
        {
            sp.size = new Vector2(base.sp.size.x, height);
            diff = (Vector2)player.transform.position - ((Vector2)tentacle.transform.position + sp.size);
            if (diff.magnitude<0.5f)
            {
                PlayerManager.Instance.EnemyHurtPlayer(this,player);
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (float height = nearestDistance; height > 0; height -= 0.2f)
        {
            sp.size = new Vector2(base.sp.size.x, height);
            yield return new WaitForSeconds(0.01f);
        }
        sp.size = new Vector2(base.sp.size.x, 0);
    }

}

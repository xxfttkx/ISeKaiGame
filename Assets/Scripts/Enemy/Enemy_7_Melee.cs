using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_7_Melee : Enemy_Melee
{
    public List<GameObject> tentacles;
    protected override void Awake()
    {
        enemy.index = 7;
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
    public override void Reset()
    {
        base.Reset();
        foreach (var t in tentacles)
        {
            var sp = t.GetComponent<SpriteRenderer>();
            sp.size = new Vector2(base.sp.size.x, 0);
        }
    }
    protected override IEnumerator Attack()
    {
        yield break;
    }
    IEnumerator Attack(GameObject tentacle)
    {
        while (true)
        {
            if (!IsAlive()) break;
            if (isBegingRepelled|| player == null)
            {
                yield return null;
                continue;
            }
            if (Utils.CanAttackPlayer(tentacle, player, GetAttackRange()))
            {
                //ATK
                var dir = player.transform.position - tentacle.transform.position;
                var angle = Vector2.Angle(Vector2.up, dir);
                if (dir.x > 0) angle = -angle;
                tentacle.transform.rotation = Quaternion.Euler(0, 0, angle);
                yield return Stretch(tentacle, dir);
            }
            else
            {

            }
            yield return new WaitForSeconds(10.0f / GetAttackSpeed());
        }
    }

    IEnumerator Stretch(GameObject tentacle, Vector2 dir)
    {
        // 0.1f 1->2
        var sp = tentacle.GetComponent<SpriteRenderer>();
        float r = GetAttackRange();
        Vector2 diff;
        dir = dir.normalized;
        for (float height = 0; height < r; height += 0.2f)
        {
            sp.size = new Vector2(sp.size.x, height);
            Vector2 ten = dir * height;
            diff = (Vector2)player.transform.position - ((Vector2)tentacle.transform.position + ten);
            if (diff.magnitude<0.5f)
            {
                PlayerManager.Instance.EnemyHurtPlayer(this,player);
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (float height = sp.size.y; height > 0; height -= 0.2f)
        {
            yield return new WaitForSeconds(0.01f);
            sp.size = new Vector2(sp.size.x, height);
            
        }
        sp.size = new Vector2(sp.size.x, 0);
    }

}

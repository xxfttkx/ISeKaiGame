using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class Utils
{ 
    public static float GetSqrDisWithPlayer(Transform curr)
    {
        Player p = PlayerManager.Instance.GetPlayerInControl();
        Vector2 dis = p.transform.position - curr.position;
        return dis.sqrMagnitude;
    }
    public static List<EnemyBase> GetNearEnemies(Vector2 curr,float range)
    {
        List<EnemyBase> enemies = new List<EnemyBase>();
        int indexMask = LayerMask.GetMask("Enemy");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(curr, range, indexMask);
        if (colliders.Length == 0) return null;
        foreach (var coll in colliders)
        {
            if (coll.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(coll.GetComponent<EnemyBase>());
            }
        }
        if (enemies.Count == 0) return null;
        for(int i = enemies.Count-1;i>=0;--i)
        {
            if (!enemies[i].IsAlive())
                enemies.RemoveAt(i);
        }
        return enemies;
    }

    public static List<EnemyBase> GetNearEnemiesByDistance(Vector2 curr, float range)
    {
        List<EnemyBase> enemies = new List<EnemyBase>();
        int indexMask = LayerMask.GetMask("Enemy");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(curr, range, indexMask);
        if (colliders.Length == 0) return null;
        foreach (var coll in colliders)
        {
            if (coll.gameObject.CompareTag("Enemy"))
            {
                var enemy = coll.GetComponent<EnemyBase>();
                enemies.Add(enemy);
            }
        }
        if (enemies.Count == 0) return null;
        for (int i = enemies.Count - 1; i >= 0; --i)
        {
            if (!enemies[i].IsAlive())
                enemies.RemoveAt(i);
        }
        enemies.Sort((e1, e2) => Mathf.RoundToInt(Mathf.Sign(Vector2.SqrMagnitude((Vector2)e1.transform.position - curr) - Vector2.SqrMagnitude((Vector2)e2.transform.position - curr))));
        return enemies;
    }
    public static List<EnemyBase> GetNearEnemiesExcludeE(Vector2 curr, float range ,EnemyBase e)
    {
        List<EnemyBase> enemies = new List<EnemyBase>();
        int indexMask = LayerMask.GetMask("Enemy");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(curr, range, indexMask);
        if (colliders.Length == 0) return null;
        foreach (var coll in colliders)
        {
            if (coll.gameObject.CompareTag("Enemy"))
            {
                var enemy = coll.GetComponent<EnemyBase>();
                if (enemy == e) continue;
                enemies.Add(enemy);
            }
        }
        if (enemies.Count == 0) return null;
        for(int i = enemies.Count-1;i>=0;--i)
        {
            if (!enemies[i].IsAlive())
                enemies.RemoveAt(i);
        }
        return enemies;
    }

    public static EnemyBase GetNearestEnemy(Vector2 curr, float range)
    {
        var enemies = GetNearEnemies(curr, range);
        if (enemies == null || enemies.Count == 0) return null;
        EnemyBase res = enemies[0];
        float dis = ((Vector2)res.transform.position - curr).sqrMagnitude;
        float newDis;
        for (int i = 1; i < enemies.Count; ++i)
        {
            newDis = ((Vector2)enemies[i].transform.position - curr).sqrMagnitude;
            if(newDis<dis)
            {
                res = enemies[i];
                dis = newDis;
            }
        }
        return res;
    }
    public static EnemyBase GetNearestEnemyExcludeE(Vector2 curr, float range, EnemyBase e)
    {
        var enemies = GetNearEnemiesExcludeE(curr, range, e);
        if (enemies == null || enemies.Count == 0) return null;
        EnemyBase res = enemies[0];
        float dis = ((Vector2)res.transform.position - curr).sqrMagnitude;
        float newDis;
        for (int i = 1; i < enemies.Count; ++i)
        {
            newDis = ((Vector2)enemies[i].transform.position - curr).sqrMagnitude;
            if (newDis < dis)
            {
                res = enemies[i];
                dis = newDis;
            }
        }
        return res;
    }

    public static bool CanAttackPlayer(EnemyBase curr, Player player)
    {
        if (player == null || !player.IsAlive()) return false;
        var disSqr = Vector2.SqrMagnitude(curr.gameObject.transform.position - player.transform.position);
        int rSqr = Mathf.FloorToInt(Mathf.Pow(curr.GetAttackRange(), 2));
        if (rSqr >= disSqr) return true;
        return false;
    }
    public static bool CanAttackPlayer(GameObject go, Player player,float range)
    {
        if (player == null || !player.IsAlive()) return false;
        var disSqr = Vector2.SqrMagnitude(go.transform.position - player.transform.position);
        float rangeSqr = range * range;
        if (rangeSqr >= disSqr) return true;
        return false;
    }

    public static bool TryAttackPlayer(GameObject go,Player player, float range)
    {
        if (player == null || !player.IsAlive()) return false;
        var disSqr = Vector2.SqrMagnitude(go.transform.position - player.transform.position);
        float rSqr = Mathf.Pow(range, 2);
        if (rSqr < disSqr) return false;
        return true;
    }

    public static bool TryFillList<T>(ref List<T> list,T dft,int count)
    {
        if (list == null)
        {
            list = Enumerable.Repeat(dft, count).ToList();
            return true;    
        }
        if(list.Count<count)
        {
            for(int i = list.Count;i<count;++i)
            {
                list.Add(dft);
            }
            return true;
        }
        return false;
    }
    public static void ShuffleArray<T>(List<T> array)
    {
        int n = array.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
    public static string GetExtraString(ExtraType extraType, int threshold)
    {
        return extraType switch
        {
            ExtraType.Hurt => $"总共对敌人造成{threshold}伤害",
            ExtraType.BeHurt => $"总共被敌人造成{threshold}伤害",
            ExtraType.Heal => $"总共对队友治疗{threshold}血量",
            ExtraType.Kill => $"总共击杀{threshold}敌人",
            ExtraType.EnterLevel => $"进入第{threshold}层",
            ExtraType.ExitLevel => $"通过第{threshold}层",
            ExtraType.EnterNum => $"总共对敌人33333造成{threshold}伤害",
            ExtraType.ExitNum => $"总共对敌人33333造成{threshold}伤害",
            _ => "Invalid day" 
        };
    }
}

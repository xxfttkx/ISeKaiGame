using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
public static class Utils
{
    public static List<EnemyBase> GetNearEnemies(Vector2 curr, float range)
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
        for (int i = enemies.Count - 1; i >= 0; --i)
        {
            if (!enemies[i].IsAlive())
                enemies.RemoveAt(i);
        }
        return enemies;
    }

    public static List<EnemyBase> GetNearEnemiesSortByDistance(Vector2 curr, float range)
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

    public static List<EnemyBase> GetNearEnemiesSortByDistance(Vector2 curr, float range, int count)
    {
        List<EnemyBase> enemies = GetNearEnemiesSortByDistance(curr, range);
        if (enemies == null || enemies.Count <= count) return enemies;
        List<EnemyBase> res = new List<EnemyBase>(count);
        for (int i = 0; i < count; ++i)
        {
            res.Add(enemies[i]);
        }
        return res;
    }
    public static List<EnemyBase> GetNearEnemiesExcludeE(Vector2 curr, float range, EnemyBase e)
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
        for (int i = enemies.Count - 1; i >= 0; --i)
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
        EnemyBase res = null;
        float minDisSqr = float.MaxValue;
        float newDis;
        foreach (var e in enemies)
        {
            newDis = (e._pos - curr).sqrMagnitude;
            if (newDis < minDisSqr)
            {
                res = e;
                minDisSqr = newDis;
            }
        }
        return res;
    }
    public static List<EnemyBase> GetSectorEnemies(Vector2 curr, Vector2 dir, float angle, float range)
    {
        var ans = GetNearEnemies(curr, range);
        if (ans == null || ans.Count == 0) return ans;
        for (int i = ans.Count - 1; i >= 0; --i)
        {
            if (Vector2.Angle(dir, ans[i]._pos - curr) > angle)
            {
                ans.RemoveAt(i);
            }
        }
        return ans;
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
    public static bool CanAttackPlayer(GameObject go, Player player, float range)
    {
        if (player == null || !player.IsAlive()) return false;
        var disSqr = Vector2.SqrMagnitude(go.transform.position - player.transform.position);
        float rangeSqr = range * range;
        if (rangeSqr >= disSqr) return true;
        return false;
    }

    public static bool TryAttackPlayer(GameObject go, Player player, float range)
    {
        if (player == null || !player.IsAlive()) return false;
        var disSqr = Vector2.SqrMagnitude(go.transform.position - player.transform.position);
        float rSqr = Mathf.Pow(range, 2);
        if (rSqr < disSqr) return false;
        return true;
    }

    public static bool TryFillList<T>(ref List<T> list, T dft, int count)
    {
        if (list == null)
        {
            list = Enumerable.Repeat(dft, count).ToList();
            return true;
        }
        if (list.Count < count)
        {
            for (int i = list.Count; i < count; ++i)
            {
                list.Add(dft);
            }
            return true;
        }
        else if (list.Count > count)
        {// Sub。。。
            for (int i = list.Count - 1; i >= count; --i)
            {
                list.RemoveAt(i);
            }
        }
        return false;
    }
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public static string GetExtraString(ExtraType extraType, int threshold)
    {
        // 124~131
        int index = extraType switch
        {
            ExtraType.Hurt => 124,
            ExtraType.BeHurt => 125,
            ExtraType.Heal => 126,
            ExtraType.Kill => 127,
            ExtraType.EnterLevel => 128,
            ExtraType.ExitLevel => 129,
            ExtraType.EnterNum => 130,
            ExtraType.ExitNum => 131,
            _ => 0,
        };
        string s = SOManager.Instance.GetStringByIndex(index);
        string result = string.Format(s, threshold);
        return result;
    }
    public static List<EnemyBase> GetEnemiesByDirAndRange(Vector2 curr, Vector2 dir, float range)
    {
        dir = dir.normalized;
        List<EnemyBase> enemies = new List<EnemyBase>();
        int indexMask = LayerMask.GetMask("Enemy");
        var rs = Physics2D.LinecastAll(curr, curr + dir * range, indexMask);
        if (rs.Length == 0) return null;
        foreach (var r in rs)
        {
            if (r.collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(r.collider.GetComponent<EnemyBase>());
            }
        }
        if (enemies.Count == 0) return null;
        for (int i = enemies.Count - 1; i >= 0; --i)
        {
            if (!enemies[i].IsAlive())
                enemies.RemoveAt(i);
        }
        return enemies;
    }
    public static string GetLanguageString(int l)
    {
        return l switch
        {
            (int)Language.Chinese => "中文",
            (int)Language.English => "English",
            (int)Language.Japanese => "日本語",
            _ => "English"
        };
    }
    public static List<string> GetLanguageList()
    {
        var list = new List<string>();
        for(int i = 0;i<(int)Language.Max;++i)
        {
            list.Add(GetLanguageString(i));
        }
        return list;
    }
    public static List<string> GetFrameRateList()
    {
        return new List<string> {"-1","30","60", "90", "120", "150", "180", "210", "240" };
    }
    public static int GetFrameRateBySelectIndex(int index)
    {
        if (index <= 0) return -1;
        else return index * 30;
    }

    public static string GetStringByCharacteristicAndVal(Characteristic c, int v, bool sign)
    {
        string t = sign ? "+" : "-";
        return c switch
        {
            Characteristic.Hp => $"hp{t}{v}",
            Characteristic.Attack => $"atk{t}{v}",
            Characteristic.Speed => $"speed{t}{v}",
            Characteristic.AttackSpeed => $"atkSpeed{t}{v}",
            Characteristic.AttackRange => $"atkRange{t}{v}",
            _ => "null"
        };
    }
    public static Vector2 Vec2Translate(Vector2 vec2, float angle)
    {
        var cos = Mathf.Cos(angle);
        var sin = Mathf.Sin(angle);
        var x = vec2.x;
        var y = vec2.y;
        return new Vector2(x * cos - y * sin, x * sin + y * cos);
    }
    public static bool JudgeListValid(List<int> l)
    {
        if (l == null || l.Count == 0) return false;
        foreach (var i in l)
        {
            if (i != -1) return true;
        }
        return false;
    }
    public static List<int> GetValidList(List<int> l)
    {
        var res = new List<int>();
        if (l == null) return res;
        foreach (var i in l)
            if (i != -1)
                res.Add(i);
        return res;
    }
    public static List<int> GetListByString(string numbersString)
    {
        // 将字符串拆分为数字字符串数组
        string[] numberStrings = numbersString.Split('.');
        // 将数字字符串数组转换为整数列表
        List<int> numbersList = numberStrings.Select(int.Parse).ToList();
        return numbersList;
    }
    public static int[] GetArrayByList(List<int> list)
    {
        if (list == null)
        {
            return new int[0];
        }
        return list.ToArray();
    }
    public static Vector2 GetVec2RotateAngle(Vector2 vec2,float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        return rotation * vec2;
    }
    public static int GetScoreByPlyaerIndexesAndLevel(List<int> playerIndexes, int level)
    {
        if (playerIndexes == null) return 0;
        return level * 100 - playerIndexes.Count;
    }
    private static readonly string key = "your_secret_key"; // 密钥，用于加密和解密
    private static readonly string iv = "your_initialization_vector"; // 初始化向量

    // 加密JSON数据
    public static void EncryptJsonData(string json, string filePath)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = Convert.FromBase64String(iv);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(json);
                    }
                }

                File.WriteAllBytes(filePath, msEncrypt.ToArray());
            }
        }
    }

    // 解密JSON数据
    public static string DecryptJsonData(string filePath)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = Convert.FromBase64String(iv);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(File.ReadAllBytes(filePath)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}

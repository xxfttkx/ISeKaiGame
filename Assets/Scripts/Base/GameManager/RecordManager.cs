using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : Singleton<RecordManager>
{
    public int level;
    public int damage;
    public int time;
    public int damageInTenSeconds;

    private void OnEnable()
    {
        EventHandler.EnterLevelEvent += OnEnterLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.EnterLevelEvent -= OnEnterLevelEvent;
    }
    public void Reset()
    {
        StopAllCoroutines();
        damage = 0;
        time = 0;
        damageInTenSeconds = 0;
    }
    public void AddDamage(int d)
    {
        damage += d;
        damageInTenSeconds += d;
        StartCoroutine(DelaySub(d));
    }

    private IEnumerator DelaySub(int d)
    {
        yield return new WaitForSeconds(10.0f);
        damageInTenSeconds -= d;
    }
    private void OnEnterLevelEvent(int l)
    {
        Reset();
        level = l;
    }
}

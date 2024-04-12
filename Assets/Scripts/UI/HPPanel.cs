using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPanel : MonoBehaviour
{
    public GameObject prefab;
    public GameObject continueHPPrefab;
    public Queue<ChangeHPUI> changeHPUIs;
    public Dictionary<Creature, HPShow> enemyToHPShow = new Dictionary<Creature, HPShow>();

    private void Start()
    {
        changeHPUIs = new Queue<ChangeHPUI>();  
    }
    private void OnEnable()
    {
        EventHandler.PlayerHurtEnemyEvent += OnPlayerHurtEnemyEvent;
        EventHandler.CreateContinueHPShowEvent += OnCreateContinueHPShowEvent;
        EventHandler.ExitLevelEvent += OnExitLevelEvent;
    }
    private void OnDisable()
    {
        EventHandler.PlayerHurtEnemyEvent -= OnPlayerHurtEnemyEvent;
        EventHandler.CreateContinueHPShowEvent -= OnCreateContinueHPShowEvent;
        EventHandler.ExitLevelEvent -= OnExitLevelEvent;
    }
    void OnPlayerHurtEnemyEvent(int atkIndex, EnemyBase e, int atk)
    {
        ChangeHPUI ui;
        if (changeHPUIs.Count > 0)
        {
            ui = changeHPUIs.Dequeue();
        }
        else
        {
            var go = Instantiate(prefab, this.transform);
            ui = go.GetComponent<ChangeHPUI>();
        }
        ui.gameObject.SetActive(true);
        ui.Init(e, atk, ()=>Release(ui));
        if (enemyToHPShow.TryGetValue(e, out HPShow hpShow))
        {
            hpShow.SetHpVal(e.GetHpVal());
        }
    }
    public void Release(ChangeHPUI ui)
    {
        changeHPUIs.Enqueue(ui);
        ui.gameObject.SetActive(false);
    }
    void OnCreateContinueHPShowEvent(Creature c, float yOffset)
    {
        var go = Instantiate(continueHPPrefab, this.transform);
        var hpShow = go.GetComponent<HPShow>();
        enemyToHPShow.Add(c, hpShow);
        hpShow.Init(c, yOffset);
    }
    void OnExitLevelEvent(int _)
    {
        enemyToHPShow.Clear();
    }
}
